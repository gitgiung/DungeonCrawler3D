using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public string playerName;
    public int level;
    public int gold;
    public int exp;
    public int currentHP;

    public List<InventoryItemSaveData> inventoryItems = new();
}

[System.Serializable]
public class InventoryItemSaveData
{
    public int itemID;
    public int count;

    public InventoryItemSaveData(int itemID, int count)
    {
        this.itemID = itemID;
        this.count = count;
    }
}

[System.Serializable]
public class GameSaveData
{
    public PlayerSaveData player;
    public InventoryItemSaveData inventory;
}