using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] private Color gizmosColor = Color.red;

    private PlayerModel model;

    public void Initialize(PlayerModel model)
    {
        this.model = model;
    }

    public void Attack()
    {
        Vector3 pos = transform.position;

        pos += transform.forward * model.AttackRange;
        pos.y += model.AttackHeight;

        Collider[] colliders =
            Physics.OverlapSphere(
                pos,
                model.AttackRadius,
                model.TargetLayer
            );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.TakeDamage(model.AttackDamage);
                DamageFontManager.Instance.CreateText(model.AttackDamage, collider.transform.position);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (model == null)
            return;

        Gizmos.color = gizmosColor;

        Vector3 pos = transform.position;

        pos += transform.forward * model.AttackRange;
        pos.y += model.AttackHeight;

        Gizmos.DrawWireSphere(
            pos,
            model.AttackRadius
        );
    }
}
