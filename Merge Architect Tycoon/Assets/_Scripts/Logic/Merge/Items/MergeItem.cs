using UnityEngine;

public enum ItemType
{
    sellable,
    unsellable,
    spawner
}

[CreateAssetMenu(menuName = "Merge Items/Merge Item", order = 2, fileName = "New Merge Item")]
public class MergeItem : ScriptableObject
{
    public string ItemName = "";
    [TextArea]
    public string ItemDescrpition = "";
    [Range(1, 7)]
    public int ItemLevel = 1;
    public float ItemCost = 0;

    public ItemType itemType= ItemType.sellable;

    public Sprite ItemSprite;

    [Space]
    public MergeItem nextItem;
    [Space]
    public int InItemsCount;
    public MergeItem InItem;
}