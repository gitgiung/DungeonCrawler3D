using System;

public static class GameEvents
{
    // Action = 일반함수(), Action<자료형> = 매개함수(변수)
    public static event Action EnemyHitEvent;
    public static event Action<int> EnemyDeadEvent;

    public static void AddEnemyHitEvent(Action action)
    {
        EnemyHitEvent += action;
    }

    public static void RemoveEnemyHitEvent(Action action)
    {
        EnemyHitEvent -= action;
    }

    public static void RaiseEnemyHit()
    {
        EnemyHitEvent?.Invoke();
    }

    public static void RaiseEnemyDead(int enemyID)
    {
        EnemyDeadEvent?.Invoke(enemyID);
    }
}
