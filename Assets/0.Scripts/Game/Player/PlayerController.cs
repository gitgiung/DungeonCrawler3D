using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(CameraController))]

// PlayerController: Model, View, States, Logic, Input Check의 허브
// StateMachine, InputReader 그리고 CameraController로 분리 가능
public class PlayerController : MonoBehaviour, IDamageable
{
    private IState currentState;

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private PlayerData data;
    public PlayerData Data => data;
    public PlayerModel Model { get; private set; }
    public PlayerView View { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerJump Jump { get; private set; }
    public PlayerDash Dash { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public CameraController CameraControl { get; private set; }

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
    public bool CanUseCameraInput =>
        GameManager.Instance != null &&
        GameManager.Instance.State == GameState.Playing &&
        (inventoryUI == null || !inventoryUI.activeInHierarchy);

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
        CameraControl = GetComponent<CameraController>();
        CameraControl.Initialize(this);

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        JumpState = new PlayerJumpState(this);
        DashState = new PlayerDashState(this);
        DeadState = new PlayerDeadState(this);
        AttackState = new PlayerAttackState(this);
        HitState = new PlayerHitState(this);

        // 어떤 상태에서든 죽는 상태로 전환하기에 여기서 구현
        Model.OnDeathStateChanged += HandleDeathStateChanged;
        Model.Init(data);


        View.Init(Model);
        Movement.Init(Data, Model);
        Jump.Initialize(Data);
        Dash.Initialize(Data);
        Combat.Initialize(Data, Model);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        CameraControl.Tick();

        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
        {
            Movement.SetMovement(Vector3.zero);
            CameraControl.SyncTarget();
            ResetInputs();
            return;
        }

        Vector3 moveDirection = CameraControl.GetMoveDirection(MoveInput);

        Movement.SetMovement(moveDirection);
        currentState?.Tick();
        Movement.Tick();
        CameraControl.SyncTarget();

        ResetInputs();
    }

    private void ResetInputs()
    {
        JumpInput = false;
        DashInput = false;
        AttackInput = false;
    }

    private void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
            JumpInput = true;
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
            DashInput = true;
    }

    private void OnAttack(InputValue value)
    {
        if (inventoryUI.activeInHierarchy)
            return;

        if (value.isPressed)
            AttackInput = true;
    }

    private void OnSprint(InputValue value)
    {
        SprintInput = value.Get<float>() > 0f;
        Movement.SetSprint(SprintInput);
    }

    public void TakeDamage(int damage)
    {
        bool damageApplied = Model.ReduceHP(damage);

        // 데미지를 받을 수 없는 상태거나 죽은 상태면 리턴
        if (!damageApplied || Model.IsDead)
            return;

        ChangeState(HitState);
    }

    private void HandleDeathStateChanged(bool isDead)
    {
        if (isDead)
        {
            ChangeState(DeadState);
            return;
        }

        // 부활 시스템 적용 가능성
        if (currentState == DeadState)
            ChangeState(IdleState);
    }

    public void ChangeState(IState newState)
    {
        if (currentState == newState)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void OnDestroy()
    {
        if (Model != null)
            Model.OnDeathStateChanged -= HandleDeathStateChanged;
    }
}
