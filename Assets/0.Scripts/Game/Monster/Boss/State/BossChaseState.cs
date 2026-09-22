using UnityEngine;

public class RegacyBossChaseState : IState
{
    private Boss boss;
    public RegacyBossChaseState(Boss boss)
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
            boss.ChangeState(new RegacyBossIdleState(boss));
            return;
        }

        float distance = Vector3.Distance(
        boss.transform.position,
        boss.Target.position
        );

        if (distance > boss.Data.LoseTargetRange)
        {
            boss.ChangeState(new RegacyBossReturnState(boss));
            return;
        }

        if (distance < boss.Data.AttackRange)
        {
            boss.ChangeState(new RegacyBossAttackState(boss));
            return;
        }

        boss.MoveTo(boss.Target.position);
    }
}
