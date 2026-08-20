using UnityEngine;

public class PlayerHitState : IState
{
    private PlayerController player;

    public PlayerHitState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Player Hit Enter");
    }

    public void Exit()
    {
        Debug.Log("Player Hit Exit");
    }

    public void Tick()
    {

    }
}
