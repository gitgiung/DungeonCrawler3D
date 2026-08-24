using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable
{
    private IState currentState;

    public MonsterIdleState IdleState { get; private set; }
    
    [Header("Targeting")]
    [SerializeField] private LayerMask targetLayer;
    public LayerMask TargetLayer
    {
        get { return targetLayer; }
    }

    [SerializeField] private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [SerializeField] private float detectRange = 4f;
    public float DetectRange
    {
        get { return detectRange; }
        set { detectRange = value; }
    }

    [SerializeField] private float loseTargetRange = 6f;
    public float LoseTargetRange
    {
        get { return loseTargetRange; }
        set { loseTargetRange = value; }
    }

    [Header("Combat")]
    [SerializeField] private int attackDamage;
    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }
    [SerializeField] private float attackDelay;
    public float AttackDelay
    {
        get { return attackDelay; }
        set { attackDelay = value; }
    }

    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public NavMeshAgent Agent { get; set; }

    public Vector3 startPos;

    private void Awake()
    {
        IdleState = new MonsterIdleState(this);
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHP = 50;
        startPos = transform.position;
        SetMoveSpeed(1f);
        ChangeState(IdleState);
    }

    private void Update()
    {
        if (Agent == null || target == null)
            return;

        currentState?.Tick();
    }

    public void Attack()
    {
        //공격 대상 판별
        Collider[] colliders = Physics.OverlapSphere(
           transform.position, 2f, targetLayer
           );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                //가까운 플레이어 타격
                target.TakeDamage(attackDamage);
                break; //단일타격. 범위 공격은 break 없애면 됨
            }
        }
    }

    public void LookAtMove(Vector3 target)
    {
        Agent.SetDestination(target);
    }

    public void SetMoveSpeed(float multiplier)
    {
        Agent.speed = moveSpeed * multiplier;
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log($"{name}이(가) 입은 피해: {damage}, {name} 체력: {currentHP}");

        if (currentHP <= 0)
        {
            Debug.Log($"{name} Dead");
            ChangeState(new MonsterDeadState(this));
        }
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    private void OnDrawGizmos()
    {
        if (currentState == IdleState)
        {
            Gizmos.color = Color.yellowGreen;
            Gizmos.DrawWireSphere(transform.position, DetectRange);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, LoseTargetRange);
        }
    }
}
