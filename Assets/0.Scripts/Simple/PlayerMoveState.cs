using UnityEngine;

public class PlayerMoveState : IState
{
    private float speed;
    private Player player;

    public PlayerMoveState(Player player, float speed)
    {
        this.speed = speed;
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("PlayerMoveState Enter");
    }

    public void Exit()
    {
        Debug.Log("PlayerMoveState Exit");
    }

    public void Tick()
    {
        Debug.Log("PlayerMoveState Tick");
        player.transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
    }
}
