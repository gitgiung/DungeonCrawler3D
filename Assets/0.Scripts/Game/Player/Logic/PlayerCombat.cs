using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Attack")]
    [SerializeField] private int atkDamage;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField, Range(0f, 3f)] private float attackHeight;
    [SerializeField, Range(0f, 3f)] private float attackRange;

    [Header("Gizmos")]
    private Color gizmosColor = Color.red;
    [SerializeField, Range(1f, 5f)] private float attackRadius = 1f;

    public void Attack()
    {
        Vector3 pos = transform.position;

        pos += transform.forward * attackRange;
        pos.y += attackHeight;

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
                target.TakeDamage(atkDamage);
                DamageFontManager.Instance.CreateText(atkDamage, collider.transform.position);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Vector3 pos = transform.position;

        pos += transform.forward * attackRange;
        pos.y += attackHeight;

        Gizmos.DrawWireSphere(
            pos,
            attackRadius
        );
    }
}
