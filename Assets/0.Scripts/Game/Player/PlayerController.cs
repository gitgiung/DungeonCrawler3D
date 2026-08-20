using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IState currentState;

    public PlayerMovement Movement { get; private set; }
    public PlayerJump Jump { get; private set; }
    public PlayerDash Dash { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerCondition Condition { get; private set; }
    public PlayerInteraction Interaction { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerHitState HitState { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Jump = GetComponent<PlayerJump>();
        Dash = GetComponent<PlayerDash>();
        Combat = GetComponent<PlayerCombat>();
        Condition = GetComponent<PlayerCondition>();
        Interaction = GetComponent<PlayerInteraction>();

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        JumpState = new PlayerJumpState(this);
        DashState = new PlayerDashState(this);
        DeadState = new PlayerDeadState(this);
        AttackState = new PlayerAttackState(this);
        HitState = new PlayerHitState(this);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(IState state)
    {
        if (currentState == state)
            return;

        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}