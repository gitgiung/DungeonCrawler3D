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
        Debug.Log("몬스터 복귀 시작");
        monster.SetMoveSpeed(2f);
        monster.LookAtMove(monster.startPos);
    }

    public void Exit()
    {
        Debug.Log("몬스터 복귀 중단");
        monster.SetMoveSpeed(1f);
    }

    public void Tick()
    {
        Collider[] cols = Physics.OverlapSphere(
            monster.transform.position, monster.DetectRange, monster.TargetLayer);

        if (cols.Length > 0)
        {
            monster.Target = cols[0].transform;
            monster.ChangeState(new MonsterChaseState(monster));
            return;
        }

        if (!monster.Agent.pathPending &&
           monster.Agent.remainingDistance <= monster.Agent.stoppingDistance)
        {
            monster.Agent.ResetPath();

            monster.ChangeState(new MonsterIdleState(monster));
        }
    }
}
