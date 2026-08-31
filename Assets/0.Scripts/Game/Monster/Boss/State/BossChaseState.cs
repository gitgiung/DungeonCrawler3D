using UnityEngine;

public class BossChaseState : IState
{
    private Boss boss;
    public BossChaseState(Boss boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(new BossIdleState(boss));
            return;
        }

        float distance = Vector3.Distance(
        boss.transform.position,
        boss.Target.position
        );

        if (distance > boss.Data.LoseTargetRange)
        {
            boss.ChangeState(new BossReturnState(boss));
            return;
        }

        if (distance < boss.Data.AttackRange)
        {
            boss.ChangeState(new BossAttackState(boss));
            return;
        }

        boss.MoveTo(boss.Target.position);
    }
}
