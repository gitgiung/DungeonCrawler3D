using UnityEngine;

public class UIDebug : MonoBehaviour
{
    [SerializeField] PlayerModel model;
    [SerializeField] Transform target;

    private void HPDown()
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable player))
        {
            player.TakeDamage(20);
        }
    }

    private void EXPUP()
    {
        model.AddExp(20);
    }

    private void LevelUP()
    {

    }

    private void AddGold()
    {
        model.AddGold(20);
    }
}
