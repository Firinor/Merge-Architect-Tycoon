using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRenderer : MonoBehaviour
{
    public TextMeshProUGUI QuestText;
    public Image QuestiImage;
    public List<QuestRewardRenderer> RewardRenderers;
    public List<QuestObjectiveRenderer> ObjectiveRenderers;
    public Button ClaimButton;

    public QuestPanel Panel;

    [Space]
    [SerializeField] private RectTransform questRewardRendererParent;
    [SerializeField] private QuestRewardRenderer questRewardRendererPrefab;
    [SerializeField] private RectTransform questObjectiveRendererParent;
    [SerializeField] private QuestObjectiveRenderer questObjectiveRendererPrefab;

    [Space]
    public QuestData CurrentData;
    private QuestInfo info => CurrentData.QuestInfo;

    public void Render(QuestData quest = null)
    {
        if(quest != null) CurrentData = quest;

        DisableAllDetails();
        RenderQuestHeader();
        RenderDetails();
        SetButtons();
    }

    private void SetButtons()
    {
        ClaimButton.gameObject.SetActive(CurrentData.IsQuestComplete());
    }

    private void RenderQuestHeader()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        QuestText.text = info.Discription;
        QuestiImage.sprite = info.Sprite;
    }
    private void RenderDetails()
    {
        for (int i = 0; i < info.RewardList.Count; i++)
        {
            RenderRewardElement(i);
        }
        for (int i = 0; i < info.Objectives.Count; i++)
        {
            RenderObjectiveElement(i);
        }
    }

    private void DisableAllDetails()
    {
        for (int i = 0; i < RewardRenderers.Count; i++)
        {
            RewardRenderers[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ObjectiveRenderers.Count; i++)
        {
            ObjectiveRenderers[i].gameObject.SetActive(false);
        }
    }

    private void RenderObjectiveElement(int i)
    {
        if (i >= ObjectiveRenderers.Count)
        {
            QuestObjectiveRenderer element = Instantiate(questObjectiveRendererPrefab, questObjectiveRendererParent);
            ObjectiveRenderers.Add(element);
        }

        if (i < info.Objectives.Count)
        {
            ObjectiveRenderers[i].gameObject.SetActive(true);
            ObjectiveRenderers[i].RenderObjective(CurrentData.ProgressList[i], info.Objectives[i], CurrentData.IsObjectiveComplete(i));
        }
    }
    private void RenderRewardElement(int i)
    {
        if (info.RewardList[i].IsHiden)
            return;

        if (i >= RewardRenderers.Count)
        {
            QuestRewardRenderer element = Instantiate(questRewardRendererPrefab, questRewardRendererParent);
            RewardRenderers.Add(element);
        }

        if (i < info.RewardList.Count)
        {
            RewardRenderers[i].gameObject.SetActive(true);
            RewardRenderers[i].RenderReward(info.RewardList[i].Amount.ToString(), info.RewardList[i].Sprite);
        }
    }

    public void ClaimReward()
    {
        CurrentData.ClaimQuestReward();
        Panel.Refresh();
    }
}