[System.Serializable]
public class PlayerSaveData
{
    public string playerName;
    public int level;
    public int gold;
    public int exp;
    public int currentHP;
}

[System.Serializable]
public class InventoryItemSaveData
{
    public int itemID;
    public int count;
}

[System.Serializable]
public class GameSaveData
{
    public PlayerSaveData player;
    public InventoryItemSaveData inventory;
}