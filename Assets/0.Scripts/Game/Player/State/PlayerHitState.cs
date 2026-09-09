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
        player.Movement.SetCanMove(false);
        player.Interaction.enabled = false;
        player.View.PlayHit();
    }

    public void Exit()
    {
        player.Interaction.enabled = true;
    }

    public void Tick()
    {
        if (!player.View.IsHitAnimationFinished())
            return;

        if (!player.Jump.IsGround)
            return;

        player.ChangeState(
            player.HasMoveInput
                ? player.MoveState
                : player.IdleState
        );
    }
}
