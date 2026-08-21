using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerController player;

    private float attackTimer;
    private float attackDuration = 1f;

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Attack Enter");
        attackTimer = attackDuration;

        player.Combat.Attack();
    }

    public void Exit()
    {
        Debug.Log("Attack Exit");
    }

    public void Tick()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            player.ChangeState(player.IdleState);
        }
    }
}
