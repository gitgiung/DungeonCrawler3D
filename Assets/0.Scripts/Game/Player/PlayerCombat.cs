using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Attack")]
    [SerializeField] private int atkDamage;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField, Range(0f, 3f)] private float attackHeight;
    [SerializeField, Range(0f, 3f)] private float attackRange;

    [Header("Gizmos")]
    public Color gizmosColor = Color.red;
    [SerializeField, Range(1f, 5f)] private float attackRadius = 1f;

    public bool HasAttack { get; private set; }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        HasAttack = true;

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
                target.TakeDamage(atkDamage);
                DamageFontManager.Instance.CreateText(atkDamage, collider.transform.position);
                break;
            }
        }

        HasAttack = false;
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
