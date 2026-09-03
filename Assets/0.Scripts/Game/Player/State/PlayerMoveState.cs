using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerController player;

    public PlayerMoveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.View.PlayMove();
        player.Movement.SetCanMove(true);
    }

    public void Exit()
    {
        player.Movement.SetCanMove(false);
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

        if (!player.HasMoveInput)
            player.ChangeState(player.IdleState);
    }
}
