using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerData data;

    public void Initialize(PlayerData data)
    {
        this.data = data;
    }

    public void Attack()
    {
        Vector3 pos = transform.position;

        pos += transform.forward * data.AttackRange;
        pos.y += data.AttackHeight;

        Collider[] colliders =
            Physics.OverlapSphere(
                pos,
                data.AttackRadius,
                data.TargetLayer
            );

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.TakeDamage(data.AttackDamage);
                DamageFontManager.Instance.CreateText(data.AttackDamage, collider.transform.position);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(data == null)
            return;

        Gizmos.color = Color.red;

        Vector3 pos = transform.position;

        pos += transform.forward * data.AttackRange;
        pos.y += data.AttackHeight;

        Gizmos.DrawWireSphere(
            pos,
            data.AttackRadius
        );
    }
}
