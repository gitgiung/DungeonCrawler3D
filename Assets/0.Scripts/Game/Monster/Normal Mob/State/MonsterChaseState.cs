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
        if (monster.Model.Target == null)
        {
            monster.ChangeState(new MonsterIdleState(monster));
            return;
        }

        float distance = Vector3.Distance(
        monster.transform.position,
        monster.Model.Target.position
        );

        if (distance > monster.Data.LoseTargetRange)
        {
            monster.ChangeState(new MonsterPatrolState(monster));
            return;
        }

        if (distance < monster.Data.AttackRange)
        {
            monster.ChangeState(new MonsterAttackState(monster));
            return;
        }

        monster.MoveTo(monster.Model.Target.position);
    }
}
