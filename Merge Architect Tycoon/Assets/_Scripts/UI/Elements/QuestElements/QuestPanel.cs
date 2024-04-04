using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public List<QuestRenderer> ElementsPool;
    [SerializeField] private RectTransform questRendererParent;
    [SerializeField] private QuestRenderer questRendererPrefab;

    public void CloseQuestElements()
    {
        foreach (QuestRenderer questElement in ElementsPool)
        {
            questElement.gameObject.SetActive(false);
        }
    }

    public void SetQuestElement(QuestData quest)
    {
        QuestRenderer questElement = ElementsPool.Find(x => x.CurrentData == quest);
        if (questElement != null)
        {
            questElement.gameObject.SetActive(true);
            return;
        }

        questElement = ElementsPool.Find(x => !x.gameObject.activeSelf);
        if(questElement == null)
            questElement = Instantiate(questRendererPrefab, questRendererParent);
        
        questElement.Show(quest);
    }
}