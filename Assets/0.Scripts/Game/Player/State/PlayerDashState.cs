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
        player.Dash.StartDash();
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if(!player.Dash.IsDashing)
        {
            player.ChangeState(player.IdleState);
        }
    }
}
