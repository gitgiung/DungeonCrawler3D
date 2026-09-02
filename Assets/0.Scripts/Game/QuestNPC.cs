using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestData questData;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestDialogUI questDialogUI;

    public string GetInteractionText()
    {
        //QuestProgress quest = questManager.GetQuest(questData.questID);

        //if (quest == null)
        //    return "퀘스트 확인";
        //if (quest.State == QuestState.CanComplete)
        //    return "퀘스트 완료";
        //if (quest.State == QuestState.InProgress)
        //    return $"진행도: {quest.CurrentCount} / {questData.requiredCount}";

        return "대화하기";
    }

    public void Interact()
    {
        QuestProgress quest =
            questManager.GetQuest(questData.questID);

        // 아직 받지 않은 퀘스트
        if (quest == null)
        {
            questDialogUI.Open(questData, AcceptQuest);
            return;
        }

        // 완료 조건을 달성한 퀘스트
        if (quest.State == QuestState.CanComplete)
        {
            questManager.CompleteQuest(questData.questID);
        }
    }

    private void AcceptQuest()
    {
        questManager.AcceptQuest(questData);
    }
}
