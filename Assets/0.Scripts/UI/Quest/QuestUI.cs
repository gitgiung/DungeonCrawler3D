using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;

    private void Start()
    {
        questManager.OnQuestChanged += ReFresh;
        ReFresh();
    }

    private void OnDestroy()
    {
        if (questManager != null)
            questManager.OnQuestChanged -= ReFresh;
    }

    private void ReFresh()
    {
        if (questManager.ActiveQuests.Count == 0)
        {
            titleText.text = string.Empty;
            progressText.text = string.Empty;

            return;
        }

        QuestProgress quest = questManager.ActiveQuests[0];
        titleText.text = quest.Data.questTitle;

        switch (quest.State)
        {
            case QuestState.InProgress:
                progressText.text = $"퀘스트 진행도: {quest.CurrentCount} / {quest.Data.requiredCount}";
                break;

            case QuestState.CanComplete:
                progressText.text = $"{quest.Data.questTitle} 완료. 보상을 받으세요";
                break;

            case QuestState.Completed:
                progressText.text = "퀘스트 완료";
                break;

        }
    }
}
