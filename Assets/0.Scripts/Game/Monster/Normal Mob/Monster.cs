using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable
{
    private IState currentState;
    [SerializeField] private EnemyData data;
    public EnemyData Data { get { return data; } }
    public MonsterModel Model { get; private set; }
    public MonsterView View { get; private set; }
    public MonsterIdleState IdleState { get; private set; }
    public NavMeshAgent Agent { get; set; }

    public Vector3 StartPos { get; private set; }

    private void Awake()
    {
        IdleState = new MonsterIdleState(this);

        Model = GetComponent<MonsterModel>();
        View = GetComponent<MonsterView>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Model.Initialize(data.MaxHP);
        View.Initialize(this);

        StartPos = transform.position;
        SetMoveSpeed(1f);
        ChangeState(IdleState);
    }

    private void Update()
    {
        currentState?.Tick();
    }

    public void Attack()
    {
        //공격 대상 판별
        Collider[] colliders = Physics.OverlapSphere(
           transform.position, Data.AttackRange, Data.TargetLayer
           );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                //가까운 플레이어 타격
                target.TakeDamage(data.AttackDamage);
                break; //단일타격. 범위 공격은 break 없애면 됨
            }
        }
    }

    private bool isDead;
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        Model.ReduceHP(damage);
        View.UpdateHP();
        Debug.Log($"{name}이(가) 입은 피해: {damage}, {name} 남은 체력: {Model.CurrentHP}");

        if (Model.IsDead)
        {
            isDead = true;
            Debug.Log($"{name} Dead");

            GameEvents.RaiseEnemyDead(Data.MonsterID);

            ChangeState(new MonsterDeadState(this));
            return;
        }

        ChangeState(new MonsterHitState(this));
    }

    public void Death()
    {
        Destroy(transform.gameObject, 5f);
    }

    public void MoveTo(Vector3 target)
    {
        Agent.SetDestination(target);
    }

    public void SetMoveSpeed(float multiplier)
    {
        Agent.speed = data.MoveSpeed * multiplier;
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}
