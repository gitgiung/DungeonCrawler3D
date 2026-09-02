using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;

    private void OnEnable()
    {
        if (questManager == null)
            return;

        questManager.OnQuestChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (questManager != null)
            questManager.OnQuestChanged -= Refresh;
    }

    private void Refresh()
    {
        QuestProgress quest = FindUnfinishedQuest();

        if (quest == null)
        {
            Clear();
            return;
        }

        titleText.text = quest.Data.questTitle;

        switch (quest.State)
        {
            case QuestState.InProgress:
                progressText.text = $"퀘스트 진행도: {quest.CurrentCount} / {quest.Data.requiredCount}";
                break;

            case QuestState.CanComplete:
                progressText.text = $"{quest.Data.questTitle} 완료";
                break;

            case QuestState.Completed:
                progressText.text = "퀘스트 완료";
                break;

        }
    }

    private QuestProgress FindUnfinishedQuest()
    {
        foreach (QuestProgress quest
                 in questManager.ActiveQuests)
        {
            if (quest.State != QuestState.Completed)
                return quest;
        }

        return null;
    }


    private void Clear()
    {
        titleText.text = string.Empty;
        progressText.text = string.Empty;
    }
}
