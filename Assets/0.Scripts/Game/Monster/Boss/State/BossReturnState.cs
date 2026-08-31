using UnityEngine;

public class BossReturnState : IState
{
    private Boss boss;
    public BossReturnState(Boss boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.SetMoveSpeed(2f);
        boss.MoveTo(boss.StartPos);
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        Collider[] cols = Physics.OverlapSphere(
            boss.transform.position, boss.Data.DetectRange, boss.Data.TargetLayer);

        if (cols.Length > 0)
        {
            boss.Target = cols[0].transform;
            boss.ChangeState(new BossChaseState(boss));
            return;
        }

        // 경로 계산이 끝났고, 목적지까지 남은 거리가 내가 정한 정지 거리보다 작은지 체크
        if (!boss.Agent.pathPending &&
           boss.Agent.remainingDistance <= boss.Agent.stoppingDistance)
        {
            boss.Agent.ResetPath(); //가지고있는 이동경로 제거

            boss.ChangeState(new BossIdleState(boss));
        }
    }
}
