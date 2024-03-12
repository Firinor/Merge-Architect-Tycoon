using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;


public class QuestsProvider : IInitializableOnSceneLoaded
{
    private readonly List<Quest> _activeQuests = new();
    public List<Quest> GetActiveQuestsList => _activeQuests;
    private readonly ReactiveCollection<Quest> _questsWaitingForClaim = new();
    public ReactiveCollection<Quest> GetQuestsWaitingForClaim => _questsWaitingForClaim;
    private IPlayerProgressService _playerProgressService;

    [Inject]
    void Construct(IPlayerProgressService playerProgressService)
    {
        _playerProgressService = playerProgressService;
    }

    public void OnSceneLoaded()
    {
        _playerProgressService.Quests.SubscribeToMerge(CheckAllQuestsCompleted);
    }

    public void ActivateQuest(Quest quest)
    {
        if (!_activeQuests.Contains(quest))
        {
            _activeQuests.Add(quest);
            _playerProgressService.Quests.AddActiveQuest(quest.questId);
            quest.InitializeRewardsAndItems();
        }
    }

    public void AddQuestWaitingForClaim(Quest quest)
    {
        quest.InitializeRewardsAndItems();
        _questsWaitingForClaim.Add(quest);
    }

    public void AddActiveQuest(Quest quest)
    {
        _activeQuests.Add(quest);
        quest.InitializeRewardsAndItems();
    }

    public void CheckAllQuestsCompleted()
    {
        List<Quest> completedQuests = _activeQuests.FindAll(quest => quest.IsCompleted(_playerProgressService));
        foreach (Quest completedQuest in completedQuests)
        {
            _questsWaitingForClaim.Add(completedQuest);
            _activeQuests.Remove(completedQuest);
            _playerProgressService.Quests.AddQuestWaitingForClaim(completedQuest.questId);
        }
    }

    public void ClaimQuestReward(Quest quest)
    {
        quest.GiveReward(_playerProgressService.Progress);
        _playerProgressService.Quests.AddCompletedQuest(quest.questId);

        _questsWaitingForClaim.Remove(quest);
    }
}