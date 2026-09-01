public class QuestProgress
{
    public QuestData Data { get; private set; }

    public int CurrentCount { get; private set; }

    public QuestState State { get; private set; }

    public QuestProgress(QuestData data)
    {
        Data = data;
        CurrentCount = 0;
        State = QuestState.InProgress;
    }

    public void AddProgress(int amount = 1)
    {
        if (State != QuestState.InProgress)
            return;

        CurrentCount += amount;
        if (CurrentCount >= Data.requiredCount)
        {
            CurrentCount = Data.requiredCount;
            State = QuestState.CanComplete;
        }
    }

    public void QuestComplete()
    {
        if (State != QuestState.CanComplete)
            return;

        State = QuestState.Completed;
    }
}
