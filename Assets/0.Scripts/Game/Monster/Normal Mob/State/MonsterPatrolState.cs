using UnityEngine;

public class MonsterPatrolState : IState
{
    private Monster monster;
    public MonsterPatrolState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.SetMoveSpeed(2f); // 이동속도 두 배 증가
        monster.View.PlayPatrol();
        monster.MoveTo(monster.StartPos);
    }

    public void Exit()
    {
        monster.SetMoveSpeed(1f);
    }

    public void Tick()
    {
        Collider[] cols = Physics.OverlapSphere(
            monster.transform.position, monster.Data.DetectRange, monster.Data.TargetLayer);

        if (cols.Length > 0)
        {
            monster.Model.Target = cols[0].transform;
            monster.ChangeState(new MonsterChaseState(monster));
            return;
        }

        // 경로 계산이 끝났고, 목적지까지 남은 거리가 내가 정한 정지 거리보다 작은지 체크
        if (!monster.Agent.pathPending &&
           monster.Agent.remainingDistance <= monster.Agent.stoppingDistance)
        {
            monster.Agent.ResetPath(); //가지고있는 이동경로 제거

            monster.ChangeState(new MonsterIdleState(monster));
        }
    }
}
