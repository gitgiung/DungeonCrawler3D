using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerController player;

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {

        player.View.PlayAttack();

        player.Combat.Attack();
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        if (player.View.IsAnimationFinished())
        {
            player.ChangeState(player.IdleState);
        }
    }
}
