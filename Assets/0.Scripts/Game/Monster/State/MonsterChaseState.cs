using UnityEngine;

public class MonsterChaseState : IState
{
    private Monster monster;
    public MonsterChaseState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        Debug.Log("몬스터 추적 진입");
        monster.View.PlayChase();
    }

    public void Exit()
    {
        Debug.Log("몬스터 추적 해제");
    }

    public void Tick()
    {
        if (monster.Target == null)
        {
            monster.ChangeState(new MonsterIdleState(monster));
            return;
        }

        float distance = Vector3.Distance(
        monster.transform.position,
        monster.Target.position
        );

        if (distance > monster.LoseTargetRange)
        {
            monster.ChangeState(new MonsterPatrolState(monster));
            return;
        }

        if (distance < 2f)
        {
            monster.ChangeState(new MonsterAttackState(monster));
            return;
        }

        monster.LookAtMove(monster.Target.position);
    }
}
