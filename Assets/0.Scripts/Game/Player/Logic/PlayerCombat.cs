using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerData data;
    private PlayerModel model;

    public void Initialize(PlayerData data, PlayerModel model)
    {
        this.data = data;
        this.model = model;
    }

    public void Attack()
    {
        if (data == null ||  model == null)
            return;

        int damage = model.AttackDamage;
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
                target.TakeDamage(damage);
                DamageFontManager.Instance.CreateText(damage, collider.transform.position);
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
