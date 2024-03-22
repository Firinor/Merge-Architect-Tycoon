using System.Linq;
using Zenject;

public class QuestGiver : IInitializableOnSceneLoaded
{
    private int _tutorialQuestsCount;

    private QuestsProvider _questsProvider;
    private IStaticDataService _staticDataService;
    private IPlayerProgressService _playerProgressService;

    [Inject]
    void Construct(QuestsProvider questsProvider, IStaticDataService staticDataService,
        IPlayerProgressService playerProgressService)
    {
        _questsProvider = questsProvider;
        _staticDataService = staticDataService;
        _playerProgressService = playerProgressService;
    }

    public void OnSceneLoaded()
    {
        ActivateQuestsOnStart();

        _tutorialQuestsCount =
            _staticDataService.Quests.Count(quest => quest.giveQuestCondition == GiveQuestCondition.Tutorial);

        _playerProgressService.Quests.SubscribeToQuestCompleted(CheckBaseQuestsActivation);

        // _playerProgressService.Quests.SubscribeToQuestValueChanged(CheckBaseQuestsActivation);
        // CheckAllQuestsForActivation();
        // _questsProvider.GetQuestsWaitingForClaim.ObserveRemove().Subscribe(_ => { CheckBaseQuestsActivation(); });
    }

    public void CheckAllQuestsForActivation()
    {
        foreach (QuestBase quest in _staticDataService.Quests)
        {
            if (!_questsProvider.GetActiveQuestsList.Contains(quest) &&
                !_questsProvider.GetQuestsWaitingForClaim.Contains(quest) &&
                !_playerProgressService.Quests.CompletedQuests.Contains(quest.questId))
            {
                if (quest.IsReadyToStart(_playerProgressService))
                {
                    _questsProvider.ActivateQuest(quest);
                }
            }
        }
    }

    private void CheckBaseQuestsActivation()
    {
        if (_tutorialQuestsCount > _playerProgressService.Quests.CompletedQuests.Count)
            return;
        CheckAllQuestsForActivation();
    }

    private void ActivateQuestsOnStart()
    {
        var questProgress = _playerProgressService.Quests;

        foreach (var quest in _staticDataService.Quests)
        {
            if (questProgress.ActiveQuests.Contains(quest.questId))
            {
                _questsProvider.AddActiveQuest(quest);
            }
            else if (questProgress.QuestsWaitingForClaim.Contains(quest.questId))
            {
                _questsProvider.AddQuestWaitingForClaim(quest);
            }
        }
    }
}