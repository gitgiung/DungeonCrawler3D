using UnityEngine;

public class MonsterChaseState : IState
{
    private readonly Monster monster;

    public MonsterChaseState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.SetMoveSpeed(1f);
        monster.ResumeMoving();
        monster.View.PlayChase();
    }

    public void Tick()
    {
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

        if (distance <= monster.Data.AttackRange)
        {
            monster.ChangeState(monster.AttackState);
            return;
        }

        monster.MoveTo(monster.Model.Target.position);
    }

    public void Exit()
    {
    }
}