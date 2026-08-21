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
        Debug.Log("점프 시작");
        player.Jump.Jump();
    }

    public void Exit()
    {
        Debug.Log("점프 끝");
    }

    public void Tick()
    {
        if (player.Jump.IsGround)
        {
            player.ChangeState(player.IdleState);
        }


    }
}
