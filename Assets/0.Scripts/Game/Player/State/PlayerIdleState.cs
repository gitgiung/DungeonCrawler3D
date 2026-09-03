using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.View.PlayIdle();
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if (player.JumpInput && player.Jump.IsGround)
        {
            player.ChangeState(player.JumpState);
            return;
        }

        if (player.DashInput && player.Jump.IsGround)
        {
            player.ChangeState(player.DashState);
            return;
        }

        if (player.AttackInput)
        {
            player.ChangeState(player.AttackState);
            return;
        }

        if (player.HasMoveInput)
            player.ChangeState(player.MoveState);
    }
}
