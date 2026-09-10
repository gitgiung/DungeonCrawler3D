using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MonsterModel))]
[RequireComponent(typeof(MonsterView))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Monster : MonoBehaviour, IDamageable
{
    private IState currentState;

    [SerializeField] private EnemyData data;
    public EnemyData Data => data;

    public MonsterModel Model { get; private set; }
    public MonsterView View { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    public MonsterIdleState IdleState { get; private set; }
    public MonsterChaseState ChaseState { get; private set; }
    public MonsterAttackState AttackState { get; private set; }
    public MonsterHitState HitState { get; private set; }
    public MonsterPatrolState PatrolState { get; private set; }
    public MonsterDeadState DeadState { get; private set; }

    public Vector3 StartPos { get; private set; }

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;
    private bool isDead;

    private void Awake()
    {
        Model = GetComponent<MonsterModel>();
        View = GetComponent<MonsterView>();
        Agent = GetComponent<NavMeshAgent>();

        body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Enemy의 이동은 NavMeshAgent가 전담한다.
        body.isKinematic = true;
        body.useGravity = false;

        CreateStates();
    }

    private void Start()
    {
        if (data == null)
        {
            Debug.LogError($"{name}에 EnemyData가 없습니다.", this);
            enabled = false;
            return;
        }

        Model.Initialize(data.MaxHP);
        View.Initialize(this, Model);

        StartPos = transform.position;

        SetMoveSpeed(1f);
        ChangeState(IdleState);
    }

    private void Update()
    {
        currentState?.Tick();
    }

    private void CreateStates()
    {
        IdleState = new MonsterIdleState(this);
        ChaseState = new MonsterChaseState(this);
        AttackState = new MonsterAttackState(this);
        HitState = new MonsterHitState(this);
        PatrolState = new MonsterPatrolState(this);
        DeadState = new MonsterDeadState(this);
    }

    public void ChangeState(IState nextState)
    {
        if (nextState == null)
            return;

        currentState?.Exit();

        currentState = nextState;

        currentState.Enter();
    }

    public void Attack()
    {
        Vector3 center = GetAttackCenter();

        Vector3 halfExtents = new Vector3(
            data.AttackWidth * 0.5f,
            data.AttackHeight * 0.5f,
            data.AttackRange * 0.5f
        );

        Collider[] colliders = Physics.OverlapBox(
            center,
            halfExtents,
            transform.rotation,
            data.TargetLayer
        );

        foreach (Collider collider in colliders)
        {
            IDamageable target =
                collider.GetComponentInParent<IDamageable>();

            if (target == null)
                continue;

            // Enemy 자신의 계층에 포함된 Collider는 제외한다.
            if (target is Component targetComponent &&
                targetComponent.transform.root == transform.root)
            {
                continue;
            }

            target.TakeDamage(data.AttackDamage);
            break;
        }
    }

    private Vector3 GetAttackCenter()
    {
        return transform.position
               + transform.forward * (data.AttackRange * 0.5f)
               + Vector3.up * (data.AttackHeight * 0.5f);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        Model.ReduceHP(damage);

        View.PlayDamageFlash();

        // 사망은 공격 애니메이션보다 우선한다
        if (Model.IsDead)
        {
            isDead = true;

            GameEvents.RaiseEnemyDead(data.MonsterID);

            ChangeState(DeadState);
            return;
        }

        // 공격 애니메이션 중에는 HitState로 전환하지 않는다
        if (object.ReferenceEquals(currentState, AttackState) &&
            AttackState.IsAnimationLocked)
        {
            return;
        }

        ChangeState(HitState);
    }

    public void MoveTo(Vector3 destination)
    {
        if (!CanUseAgent())
            return;

        Agent.SetDestination(destination);
    }

    //  Chase, Patrol 제외하고는 Nav 사용 중단
    public void StopMoving()
    {
        if (!CanUseAgent())
            return;

        Agent.isStopped = true;
        Agent.ResetPath();
    }

    public void ResumeMoving()
    {
        if (!CanUseAgent())
            return;

        Agent.isStopped = false;
    }

    public void SetMoveSpeed(float multiplier)
    {
        if (Agent == null)
            return;

        Agent.speed = data.MoveSpeed * multiplier;
    }

    // 완전시 Death()호출 전 Agent, collider, rigidbody 비활성화
    public void PrepareForDeath()
    {
        StopMoving();

        if (Agent != null && Agent.enabled)
            Agent.enabled = false;

        if (body != null)
        {
            if (!body.isKinematic)
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }

            body.isKinematic = true;
            body.useGravity = false;
        }

        if (capsuleCollider != null)
            capsuleCollider.enabled = false;
    }

    public void Death()
    {
        Destroy(gameObject, 5f);
    }

    // Nav 사용할지 말지 결정
    private bool CanUseAgent()
    {
        return Agent != null &&
               Agent.enabled &&
               Agent.isOnNavMesh;
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null)
            return;

        Matrix4x4 previousMatrix = Gizmos.matrix;
        Color previousColor = Gizmos.color;

        Gizmos.color = Color.red;

        // Enemy의 회전 방향을 포함하는 로컬 좌표계를 사용한다.
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position,
            transform.rotation,
            Vector3.one
        );

        Vector3 localCenter = new Vector3(
            0f,
            data.AttackHeight * 0.5f,
            data.AttackRange * 0.5f
        );

        Vector3 size = new Vector3(
            data.AttackWidth,
            data.AttackHeight,
            data.AttackRange
        );

        Gizmos.DrawWireCube(localCenter, size);

        Gizmos.matrix = previousMatrix;
        Gizmos.color = previousColor;
    }
}