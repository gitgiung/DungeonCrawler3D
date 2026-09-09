using UnityEngine;

public class PlayerDeadState : IState
{
    private PlayerController player;

    public PlayerDeadState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.SetCanMove(false);
        player.Dash.StopDash();
        player.Interaction.enabled = false;
        player.View.PlayDead();
    }

    public void Exit()
    {
        player.Interaction.enabled = true;
    }

    public void Tick()
    {
    }
}
