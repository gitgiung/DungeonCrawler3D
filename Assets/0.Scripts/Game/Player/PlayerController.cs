using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerDash))]
public class PlayerController : MonoBehaviour, IDamageable
{
    private IState currentState;

    [SerializeField] private PlayerData data;
    public PlayerData Data => data;
    public PlayerModel Model { get; private set; }
    public PlayerView View { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerJump Jump { get; private set; }
    public PlayerDash Dash { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerInteraction Interaction { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerHitState HitState { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool HasMoveInput => MoveInput.sqrMagnitude > 0.001f;
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool SprintInput { get; private set; }

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogError("PlayerData is not assigned.", this);
            enabled = false;
            return;
        }

        Model = GetComponent<PlayerModel>();
        View = GetComponent<PlayerView>();
        Movement = GetComponent<PlayerMovement>();
        Jump = GetComponent<PlayerJump>();
        Dash = GetComponent<PlayerDash>();
        Combat = GetComponent<PlayerCombat>();
        Interaction = GetComponent<PlayerInteraction>();

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        JumpState = new PlayerJumpState(this);
        DashState = new PlayerDashState(this);
        DeadState = new PlayerDeadState(this);
        AttackState = new PlayerAttackState(this);
        HitState = new PlayerHitState(this);

        View.Initialize(Model, data);
        Movement.Initialize(data);
        Jump.Initialize(data);
        Dash.Initialize(Model, data);
        Combat.Initialize(data);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            Movement.SetMovement(Vector3.zero);
            ResetInputs();
            return;
        }

        Vector3 moveDirection = new Vector3(
            MoveInput.x,
            0f,
            MoveInput.y
        );

        Movement.SetMovement(moveDirection);
        currentState?.Tick();
        Movement.Tick();

        ResetInputs();
    }

    private void ResetInputs()
    {
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

    [SerializeField] private GameObject inventoryUI;
    public void OnAttack(InputValue value)
    {
        if (inventoryUI.activeInHierarchy)
            return;

        if (value.isPressed)
            AttackInput = true;
    }

    public void OnSprint(InputValue value)
    {
        SprintInput = value.Get<float>() > 0f;
        Movement.SetSprint(SprintInput);
    }

    public void TakeDamage(int damage)
    {
        Model.ReduceHP(damage);
        View.UpdateHP();

        Debug.Log($"Player damage: {damage}, HP: {Model.CurrentHP}");
        if (Model.IsDead)
        {
            Debug.Log($"{name} died");
            // ChangeState(DeadState);
        }
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
