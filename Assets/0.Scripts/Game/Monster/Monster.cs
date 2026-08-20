using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    private int hp, maxhp;
    [SerializeField] private int atkDmg;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform target;

    //공격 시간
    float atkDuration = 2f;
    float atkTimer = 0f;

    private void Start()
    {
        hp = maxhp = 50;

        atkTimer = atkDuration;
    }

    private void Update()
    {
        if (target == null)
            return;

        LookAtMove();
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
                target.TakeDamage(atkDmg);
                break; //단일타격. 범위 공격은 break 없애면 됨
            }
        }
    }

    private void LookAtMove()
    {
        transform.LookAt(target);

        //실무에서는 SqrMagnitude 사용할것
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 1.5f)
        {
            atkTimer -= Time.deltaTime;
            if (atkTimer <= 0f)
            {
                atkTimer = atkDuration;
                Attack();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime); //1f: 이동속도
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log($"몬스터가 입은 피해: {damage}, 몬스터 체력: {hp}");

        if (hp <= 0)
        {
            Debug.Log($"{name} Dead");
        }
    }
}
