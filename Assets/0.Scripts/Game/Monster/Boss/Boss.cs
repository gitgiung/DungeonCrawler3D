using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum BossPhase
{
    Phase1, Phase2
}

public enum BossAttackType
{
    Normal, Floor, Rush
}

public class Boss : MonoBehaviour
{
    private IState currentState;

    [SerializeField] private EnemyData data;
    public EnemyData Data => data;
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    public BossPhase Phase { get; private set; }
    public BossAttackType AttackType { get; set; }
    public Vector3 StartPos { get; private set; }

    public Transform Target { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartPos = transform.position;
        SetMoveSpeed(1f);
        Phase = BossPhase.Phase1;
        ChangeState(new BossIdleState(this));
    }

    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void SetMoveSpeed(float multiplier)
    {
        agent.speed = data.MoveSpeed * multiplier;
    }

    public void StartRush()
    {
        StartCoroutine(DashAttack());
    }

    [SerializeField] private GameObject dashWarning;
    private Vector3 dashDirection;
    private void ShowDashWarning()
    {
        dashWarning.SetActive(true);

        dashWarning.transform.position =
            transform.position
            + dashDirection * 5f;

        dashWarning.transform.rotation =
            Quaternion.LookRotation(dashDirection);
    }

    private IEnumerator DashAttack()
    {
        Vector3 targetPos = Target.position;

        dashDirection = targetPos - transform.position;
        dashDirection.y = 0;
        dashDirection.Normalize();

        transform.rotation =
            Quaternion.LookRotation(dashDirection);

        ShowDashWarning();

        yield return new WaitForSeconds(1f);

        dashWarning.SetActive(false);

        float dashDistance = 10f;
        float dashSpeed = 20f;

        Vector3 startPosition = transform.position;

        while (Vector3.Distance(
            startPosition,
            transform.position) < dashDistance)
        {
            transform.position +=
                dashDirection
                * dashSpeed
                * Time.deltaTime;

            yield return null;
        }
    }
}
