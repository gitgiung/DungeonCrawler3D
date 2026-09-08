using UnityEngine;

public class MonsterHitState : IState
{
    private readonly Monster monster;
    private float stunTimer;

    public MonsterHitState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        stunTimer = 0f;

        monster.StopMoving();
        monster.View.PlayHit();
    }

    public void Tick()
    {
        stunTimer += Time.deltaTime;

        if (stunTimer < monster.Data.StunTime)
            return;

        if (monster.Model.Target == null)
        {
            monster.ChangeState(monster.IdleState);
            return;
        }

        float distance = Vector3.Distance(
            monster.transform.position,
            monster.Model.Target.position
        );

        if (distance > monster.Data.LoseTargetRange)
        {
            monster.ChangeState(monster.PatrolState);
            return;
        }

        monster.ChangeState(monster.ChaseState);
    }

    public void Exit()
    {
    }
}