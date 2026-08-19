using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Attack")]
    [SerializeField] private int atkDmg;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField, Range(0f, 3f)] private float attackHeight;
    [SerializeField, Range(0f, 3f)] private float attackRange;

    [Header("Gizmos")]
    public Color gizmosColor = Color.red;
    [SerializeField, Range(1f, 5f)] private float attackRadius = 1f;

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector3 pos = transform.position;

        pos.y += attackHeight;
        pos.z += attackRange;

        Collider[] colliders =
            Physics.OverlapSphere(
                pos,
                attackRadius,
                targetLayer
            );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.TakeDamage(atkDmg);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Vector3 pos = transform.position;

        pos.y += attackHeight;
        pos.z += attackRange;

        Gizmos.DrawWireSphere(
            pos,
            attackRadius
        );
    }
}
