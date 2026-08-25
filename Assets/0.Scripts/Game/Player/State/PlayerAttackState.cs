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
        Debug.Log("Attack Enter");

        player.View.PlayAttack();

        player.Combat.Attack();
    }

    public void Exit()
    {
        Debug.Log("Attack Exit");
    }

    public void Tick()
    {
        if (player.View.IsAnimationFinished())
        {
            player.ChangeState(player.IdleState);
        }
    }
}
