using UnityEngine;
using UnityEngine.InputSystem;

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


    // Input
    public Vector2 MoveInput { get; private set; }
    public bool HasMoveInput =>
        MoveInput.sqrMagnitude > 0.001f;
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool SprintInput { get; private set; }

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
        if (GameManager.Instance.State != GameState.Playing)
            return;

        currentState?.Tick();

        Movement.Look();

        //한 프레임짜리 입력 초기화
        JumpInput = false;
        DashInput = false;
        AttackInput = false;
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            JumpInput = true;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed)
            DashInput = true;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
            AttackInput = true;
    }

    public void OnSprint(InputValue value)
    {
        SprintInput = value.isPressed;
    }

    public void ChangeState(IState newState)
    {
        if (currentState == newState)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}