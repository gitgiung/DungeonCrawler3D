using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestData questData;
    [SerializeField] private QuestManager questManager;

    public string GetInteractionText()
    {
        QuestProgress quest = questManager.GetQuest(questData.questID);

        if (quest == null)
            return "[F]를 눌러 퀘스트 받기";
        if (quest.State == QuestState.CanComplete)
            return "[F]를 눌러 퀘스트 완료하기";
        if (quest.State == QuestState.InProgress)
            return $"진행도: {quest.CurrentCount} / {questData.requiredCount}";

        return "[F]를 눌러 대화하기";
    }

    public void Interact()
    {
        QuestProgress quest = questManager.GetQuest(questData.questID);
        if (quest == null)
        {
            questManager.AccpetQuest(questData);
            return;
        }

        if (quest.State == QuestState.CanComplete)
        {
            questManager.CompleteQuest(questData.questID);
        }
    }
}
