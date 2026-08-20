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
        //Debug.Log("Idle Enter");
    }

    public void Exit()
    {
        //Debug.Log("Idle Exit");
    }

    public void Tick()
    {
        if (player.Movement.HasInput)
        {
            player.ChangeState(player.MoveState);
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
