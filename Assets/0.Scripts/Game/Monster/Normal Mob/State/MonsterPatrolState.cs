using UnityEngine;

public class MonsterPatrolState : IState
{
    private readonly Monster monster;

    public MonsterPatrolState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.Model.Target = null;

        monster.SetMoveSpeed(2f);
        monster.ResumeMoving();
        monster.View.PlayPatrol();
        monster.MoveTo(monster.StartPos);
    }

    public void Tick()
    {
        Collider[] colliders = Physics.OverlapSphere(
            monster.transform.position,
            monster.Data.DetectRange,
            monster.Data.TargetLayer
        );

        if (colliders.Length > 0)
        {
            monster.Model.Target = colliders[0].transform;
            monster.ChangeState(monster.ChaseState);
            return;
        }

        // 경로 계산이 끝났고, 정지 거리보다 남은 거리가 작아질 때
        // 그리고 경로 계산 상태 확인
        bool arrived =
            !monster.Agent.pathPending &&
            monster.Agent.remainingDistance <=
            monster.Agent.stoppingDistance &&
            (!monster.Agent.hasPath ||
             monster.Agent.velocity.sqrMagnitude <= 0.01f);

        if (!arrived)
            return;

        monster.ChangeState(monster.IdleState);
    }

    public void Exit()
    {
        monster.SetMoveSpeed(1f);
    }
}