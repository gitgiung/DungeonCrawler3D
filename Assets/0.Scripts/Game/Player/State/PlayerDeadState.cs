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
        Debug.Log("Player Died");
        //이후 정지할 것 처리
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if(Input.GetKeyDown(KeyCode.R) && player.Condition.HasDead)
        {
            Debug.Log("Revived");
            player.Condition.Revive();
            player.ChangeState(player.IdleState);
        }
    }
}
