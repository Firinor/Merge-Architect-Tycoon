using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ItemsCatalogue : MonoBehaviour
{
    public List<MergeItem> mergeItemsCatalogue;

    [Inject] SlotsManager slotsManager;
    [Inject] InformationPanel informationPanel;

    public int GetItemCount(MergeItem item)
    {
        return slotsManager.Slots
            .FindAll(
                s => s.CurrentItem && s.CurrentItem.name == item.name
                                   && (s.SlotState == SlotState.Draggable || s.SlotState == SlotState.Unloading))
            .Count;
    }

    public bool CheckHasItems(List<MergeItem> items)
    {
        foreach (var item in items.Distinct())
        {
            int itemCountInList = items.Count(x => x.name == item.name);
            if (itemCountInList > GetItemCount(item))
            {
                return false;
            }
        }
        return true;
    }

    public void TakeItems(List<MergeItem> items)
    {
        foreach (var item in items)
        {
            var slotItem = slotsManager.Slots.FirstOrDefault(s => s.CurrentItem &&
                                                                  s.CurrentItem.name == item.name &&
                                                                  (s.SlotState == SlotState.Unloading ||
                                                                   s.SlotState == SlotState.Draggable));
            if (slotItem != null)
            {
                if (informationPanel.slotCurrent == slotItem)
                {
                    informationPanel.ActivateInfromPanel(false);
                }

                slotItem.RemoveItem();
            }
        }
    }
}