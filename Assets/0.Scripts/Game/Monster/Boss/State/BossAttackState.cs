using UnityEngine;

public class BossAttackState : IState
{
    private Boss boss;
    public BossAttackState(Boss boss)
    {
        this.boss = boss;
    }

    private float attackTimer;
    private BossAttackType lastAttack;
    private int normalAttackCount = 0;

    public void Enter()
    {
        attackTimer = 0.5f;
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(new BossChaseState(boss));
            return;
        }

        boss.transform.LookAt(boss.Target);

        float distance = Vector3.Distance(
        boss.transform.position,
        boss.Target.position
        );

        if (distance > boss.Data.AttackRange)
        {
            boss.ChangeState(new BossChaseState(boss));
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            attackTimer = boss.Data.AttackDelay;

            SelectAttack();
        }
    }

    private void SelectAttack()
    {
        BossAttackType nextAttack;

        if (normalAttackCount >= 2)
        {
            int rand = Random.Range(0, 2);

            nextAttack = rand == 0
                ? BossAttackType.Floor
                : BossAttackType.Rush;
        }
        else
        {
            switch(lastAttack)
            {
                case BossAttackType.Normal:
                    {
                        int rand = Random.Range(0, 3);

                        nextAttack = rand switch
                        {
                            0 => BossAttackType.Floor,
                            1 => BossAttackType.Rush,
                            _ => BossAttackType.Normal
                        };
                        break;
                    }
                case BossAttackType.Floor:
                    {
                        int rand = Random.Range(0, 2);

                        nextAttack = rand == 0
                            ? BossAttackType.Rush
                            : BossAttackType.Normal;
                        break;
                    }

                case BossAttackType.Rush:
                    {
                        int rand = Random.Range(0, 2);

                        nextAttack = rand == 0
                            ? BossAttackType.Floor
                            : BossAttackType.Normal;
                        break;
                    }

                default:
                    nextAttack = BossAttackType.Normal;
                    break;
            }
        }

        ExcuteAttack(nextAttack);
    }

    private void ExcuteAttack(BossAttackType attack)
    {
        switch (attack)
        {
            case BossAttackType.Normal:
                Debug.Log("일반 공격");
                normalAttackCount++;
                break;
            case BossAttackType.Floor:
                Debug.Log("바닥 공격");
                normalAttackCount = 0;
                break;
            case BossAttackType.Rush:
                Debug.Log("돌진 공격");
                boss.StartRush();
                normalAttackCount = 0;
                break;
        }

        lastAttack = attack;
    }
}
