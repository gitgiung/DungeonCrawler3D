using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    private int amount;

    public void Initialize(int amount)
    {
        this.amount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerModel model = other.GetComponent<PlayerModel>();

        if (model == null)
            return;

        model.AddGold(amount);

        Destroy(gameObject);
    }
}