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
        //Debug.Log("Move Enter");
    }

    public void Exit()
    {
        //Debug.Log("Move Exit");
    }

    public void Tick()
    {
        if (!player.Movement.HasInput)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(player.AttackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            player.ChangeState(player.DashState);
            return;
        }
    }
}
