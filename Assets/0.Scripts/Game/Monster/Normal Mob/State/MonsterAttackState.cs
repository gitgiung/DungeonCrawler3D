using UnityEngine;

public class MonsterAttackState : IState
{
    private Monster monster;
    public MonsterAttackState(Monster monster)
    {
        this.monster = monster;
    }

    private float attackTimer = 0.5f;

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        if (monster.Model.Target == null)
        {
            monster.ChangeState(new MonsterChaseState(monster));
            return;
        }

        float distance = Vector3.Distance(
        monster.transform.position,
        monster.Model.Target.position
        );

        if (distance > monster.Data.AttackRange)
        {
            monster.ChangeState(new MonsterChaseState(monster));
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = monster.Data.AttackDelay;

            monster.View.PlayAttack();
            monster.Attack();
        }

        
    }
}
