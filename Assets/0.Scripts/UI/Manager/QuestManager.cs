using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private readonly List<QuestProgress> activeQuests = new();
    public IReadOnlyList<QuestProgress> ActiveQuests => activeQuests;
    public event Action OnQuestChanged;

    private PlayerModel playerModel;
    private void Awake()
    {
        playerModel = UnityEngine.Object.FindAnyObjectByType<PlayerModel>();
    }

    public bool AccpetQuest(QuestData quest)
    {
        if (quest == null)
            return false;

        if (HasQuest(quest.questID))
        {
            Debug.Log("이미 승낙한 퀘스트입니다.");
            return false;
        }

        QuestProgress progress = new(quest);
        activeQuests.Add(progress);
        OnQuestChanged?.Invoke();
        Debug.Log($"퀘스트 수락: {quest.questTitle}");

        return true;
    }

    public bool HasQuest(int questID)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Data.questID == questID)
                return true;
        }

        return false;
    }

    public QuestProgress GetQuest(int questID)
    {
        foreach(QuestProgress quest in activeQuests)
        {
            if (quest.Data.questID == questID)
            {
                return quest;
            }
        }

        return null;
    }

    public void NotifyEnemyKilled(int enemyID)
    {
        bool change = false;

        foreach(QuestProgress quest in activeQuests)
        {
            if (quest.State != QuestState.InProgress)
                continue;

            if (quest.Data.questType != QuestType.Kill)
                continue;

            if (quest.Data.targetID != enemyID)
                continue;

            quest.AddProgress(1);
            change = true;
            Debug.Log($"{quest.Data.questTitle}: {quest.CurrentCount} / {quest.Data.requiredCount}");
        }

        if (change == true)
        {
            OnQuestChanged?.Invoke();
        }
    }

    public bool CompleteQuest(int questID)
    {
        QuestProgress quest = GetQuest(questID);

        if (quest == null)
            return false;
        if (quest.State != QuestState.CanComplete)
            return false;

        QuestData data = quest.Data;
        if (data.rewardItem != null && data.rewardItemCount != 0)
        {
            int remaining = UIController.Instance.inventory.CreateItem(data.rewardItem, data.rewardItemCount);

            if (remaining > 0)
            {
                Debug.Log("가방 공간 부족");
                return false;
            }
        }

        playerModel.AddExp(data.rewardExp);
        playerModel.AddGold(data.rewardGold);

        quest.QuestComplete();
        OnQuestChanged?.Invoke();
        Debug.Log($"퀘스트 완료: {data.questTitle}");

        return true;
    }
}
