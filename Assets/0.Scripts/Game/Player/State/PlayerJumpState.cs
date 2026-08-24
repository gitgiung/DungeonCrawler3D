using UnityEngine;

public class PlayerJumpState : IState
{
    private PlayerController player;

    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Jump.Jump();
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        if (player.Jump.IsGround)
        {
            player.ChangeState(player.IdleState);
        }
    }
}
