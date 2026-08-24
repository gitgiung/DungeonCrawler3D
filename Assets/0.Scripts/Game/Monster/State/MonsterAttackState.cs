using UnityEngine;

public class MonsterAttackState : IState
{
    private Monster monster;
    public MonsterAttackState(Monster monster)
    {
        this.monster = monster;
    }

    private float attackTimer = 0f;

    public void Enter()
    {
        Debug.Log("몬스터의 공격 시작");
        monster.Attack();

        attackTimer = monster.AttackDelay;
    }

    public void Exit()
    {
        Debug.Log("몬스터의 공격 끝");
    }

    public void Tick()
    {
        float distance = Vector3.Distance(
        monster.transform.position,
        monster.Target.position
        );

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = monster.AttackDelay;
            monster.Attack();
        }

        if (distance > 2f)
        {
            monster.ChangeState(new MonsterChaseState(monster));
            return;
        }
    }
}
