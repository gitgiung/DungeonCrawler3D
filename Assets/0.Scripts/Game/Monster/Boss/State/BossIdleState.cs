using UnityEngine;

public class BossIdleState : IState
{
    private Boss boss;
    public BossIdleState(Boss boss)
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
        Collider[] cols = Physics.OverlapSphere(
            boss.transform.position,
            boss.Data.DetectRange,
            boss.Data.TargetLayer);

        if (cols.Length == 0)
            return;

        boss.Target = cols[0].transform;
        boss.ChangeState(new BossChaseState(boss));
    }
}
