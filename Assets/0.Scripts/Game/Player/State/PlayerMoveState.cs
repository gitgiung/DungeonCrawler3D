using Unity.IO.LowLevel.Unsafe;
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
        Debug.Log("Move Enter");
        player.Movement.SetCanMove(true);
    }

    public void Exit()
    {
        Debug.Log("Move Exit");
        player.Movement.SetCanMove(false);
        player.Movement.Stop();
    }

    public void Tick()
    {
        if (!player.HasMoveInput)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        //이동 방향 생성
        Vector3 direction = new Vector3(player.MoveInput.x, 0f, player.MoveInput.y);

        //Movement에 방향 전달
        player.Movement.SetMovement(direction);

        
        player.Movement.SetSprint(player.SprintInput);

        if (player.JumpInput && player.Jump.IsGround)
        {
            player.ChangeState(player.JumpState);
            return;
        }

        if (player.DashInput)
        {
            player.ChangeState(player.DashState);
            return;
        }

        if (player.AttackInput)
        {
            player.ChangeState(player.AttackState);
            return;
        }
    }
}
