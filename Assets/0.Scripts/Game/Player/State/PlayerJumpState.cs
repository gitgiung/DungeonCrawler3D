public class PlayerJumpState : IState
{
    private PlayerController player;

    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.SetCanMove(true);
        player.Jump.BeginJump();
    }

    public void Exit()
    {
        player.Movement.SetCanMove(false);
    }

    public void Tick()
    {
        if (!player.Jump.IsGround)
            return;

        player.ChangeState(
            player.HasMoveInput
                ? player.MoveState
                : player.IdleState
        );
    }
}
