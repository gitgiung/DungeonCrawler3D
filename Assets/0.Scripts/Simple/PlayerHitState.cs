using UnityEngine;

public class PlayerHitState : IState
{
    private int hp, maxhp, damage;
    private IState prevState;
    private Player player;

    public PlayerHitState(Player player, IState prevState, int damage)
    {
        this.prevState = prevState;
        this.player = player;
        this.damage = damage;
    }

    public void Enter()
    {
        Debug.Log("PlayerHitState Enter");
        hp -= damage;

        if(hp <= 0)
        {
            player.ChangeState(prevState);
        }
        else
        {
            //player.ChangeState(new PlayerDeadState());
        }
    }

    public void Exit()
    {
        Debug.Log("PlayerHitState Exit");
    }

    public void Tick()
    {
        Debug.Log("PlayerHitState Tick");
    }
}
