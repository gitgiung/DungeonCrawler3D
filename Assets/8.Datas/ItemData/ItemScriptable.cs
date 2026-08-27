using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment
}

[CreateAssetMenu(fileName = "ItemData")]
public class ItemScriptable : ScriptableObject
{
    [SerializeField] private int itemID;
    public int ItemID => itemID;

    [SerializeField] private ItemType itemType;
    public ItemType ItemType => itemType;

    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private uint maxStack = 1; //몇 개까지 쌓이는지
    public uint MaxStack => maxStack;

    [SerializeField] private int price;
    public int Price => price;
}
