using UnityEngine;

public enum QuestState
{
    InProgress, CanComplete, Completed
}

public enum QuestType
{
    Kill, Collect
}

[CreateAssetMenu(fileName = "QuestData", menuName = "Game/Data/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public int questID;
    public string questTitle;

    [TextArea]
    public string questDescription;

    [Header("Condition")]
    public QuestType questType;
    public int targetID;
    public int requiredCount = 1;

    [Header("Reward")]
    public int rewardGold;
    public int rewardExp;
    public ItemScriptable rewardItem;
    public int rewardItemCount;
}
