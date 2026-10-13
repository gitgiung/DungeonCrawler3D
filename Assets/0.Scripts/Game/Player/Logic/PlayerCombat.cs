
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("공격 판정")]
    [SerializeField, Range(1f, 180f)]
    private float attackAngle = 120f;

    [SerializeField]
    private LayerMask obstacleLayer;

    private PlayerData data;
    private PlayerModel model;

    public void Initialize(PlayerData data, PlayerModel model)
    {
        this.data = data;
        this.model = model;
    }

    public void Attack()
    {
        if (data == null || model == null)
            return;

        Vector3 origin = GetAttackOrigin();
        Vector3 attackCenter = GetAttackCenter();

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        float minDot = Mathf.Cos(
            attackAngle * 0.5f * Mathf.Deg2Rad
        );

        Collider[] colliders = Physics.OverlapSphere(
            attackCenter,
            data.AttackRadius,
            data.TargetLayer,
            QueryTriggerInteraction.Collide
        );

        IDamageable nearestTarget = null;
        Collider nearestCollider = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            // 플레이어 자신의 Collider 제외
            if (collider.transform.IsChildOf(transform))
                continue;

            IDamageable target =
                collider.GetComponentInParent<IDamageable>();

            if (target == null)
                continue;

            Vector3 hitPoint = collider.ClosestPoint(origin);

            // 수평 방향으로 공격 각도 계산
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            // Collider 내부에 있는 경우를 위한 보정
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = collider.bounds.center - transform.position;
                direction.y = 0f;
            }

            if (direction.sqrMagnitude > 0.0001f)
            {
                float dot = Vector3.Dot(
                    forward,
                    direction.normalized
                );

                if (dot < minDot)
                    continue;
            }

            // 장애물이 공격을 막는지 검사
            if (Physics.Linecast(
                origin,
                hitPoint,
                obstacleLayer,
                QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            float distance = (hitPoint - origin).sqrMagnitude;

            if (distance >= nearestDistance)
                continue;

            nearestDistance = distance;
            nearestTarget = target;
            nearestCollider = collider;
        }

        if (nearestTarget == null)
            return;

        // 대상이 파괴되기 전에 위치 확보
        Vector3 damagePosition = nearestCollider.bounds.center;
        int damage = model.AttackDamage;

        nearestTarget.TakeDamage(damage);

        DamageFontManager.Instance.CreateText(
            damage,
            damagePosition
        );
    }

    private Vector3 GetAttackOrigin()
    {
        return transform.position
            + Vector3.up * data.AttackHeight;
    }

    private Vector3 GetAttackCenter()
    {
        return GetAttackOrigin()
            + transform.forward * data.AttackRange;
    }

    private void OnDrawGizmos()
    {
        if (data == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            GetAttackCenter(),
            data.AttackRadius
        );

        Gizmos.color = Color.yellow;

        Vector3 origin = GetAttackOrigin();
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        float range = data.AttackRange + data.AttackRadius;

        Gizmos.DrawLine(
            origin,
            origin + forward * range
        );

        Vector3 left = Quaternion.AngleAxis(
            -attackAngle * 0.5f,
            Vector3.up
        ) * forward;

        Vector3 right = Quaternion.AngleAxis(
            attackAngle * 0.5f,
            Vector3.up
        ) * forward;

        Gizmos.DrawLine(origin, origin + left * range);
        Gizmos.DrawLine(origin, origin + right * range);
    }
}
