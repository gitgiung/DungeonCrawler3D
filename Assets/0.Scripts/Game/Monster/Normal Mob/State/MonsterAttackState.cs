using UnityEngine;

public class MonsterAttackState : IState
{
    private readonly Monster monster;

    private float attackTimer;
    private bool recoveryAnimationPlaying;

    public bool IsAnimationLocked { get; private set; }

    public MonsterAttackState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.StopMoving();

        StartAttack();
    }

    public void Tick()
    {
        attackTimer -= Time.deltaTime;

        // 공격 애니메이션이 끝날 때까지 어떤 상태 전환도 하지 않는다.
        if (IsAnimationLocked)
        {
            if (!monster.View.IsAttackAnimationFinished())
                return;

            IsAnimationLocked = false;
        }

        // 아래 상태 전환은 공격 애니메이션이 끝난 뒤에만 실행된다.
        if (monster.Model.Target == null)
        {
            monster.ChangeState(monster.IdleState);
            return;
        }

        float distance = Vector3.Distance(
            monster.transform.position,
            monster.Model.Target.position
        );

        if (distance > monster.Data.AttackRange)
        {
            monster.ChangeState(monster.ChaseState);
            return;
        }

        if (attackTimer > 0f)
        {
            // 공격 간 대기 시간에는 Idle 애니메이션을 재생한다.
            if (!recoveryAnimationPlaying)
            {
                recoveryAnimationPlaying = true;
                monster.View.PlayIdle();
            }

            return;
        }

        StartAttack();
    }

    public void Exit()
    {
        IsAnimationLocked = false;
        recoveryAnimationPlaying = false;
    }

    private void StartAttack()
    {
        monster.StopMoving();

        recoveryAnimationPlaying = false;
        IsAnimationLocked = true;

        monster.View.PlayAttack();
        monster.Attack();

        attackTimer = monster.Data.AttackDelay;
    }
}