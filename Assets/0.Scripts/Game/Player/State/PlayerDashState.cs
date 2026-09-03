using UnityEngine;

public class PlayerDashState : IState
{
    private PlayerController player;

    public PlayerDashState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Dash.StartDash(player.Movement.LastLookDirection);
    }

    public void Exit()
    {
        player.Dash.StopDash();
    }

    public void Tick()
    {
        if (player.Dash.IsDashing)
            return;

        player.ChangeState(
            player.HasMoveInput
                ? player.MoveState
                : player.IdleState
        );
    }
}
