using UnityEngine;

public class GoldPickupTrigger : MonoBehaviour
{
    private GoldPickup goldPickup;

    private void Awake()
    {
        goldPickup = GetComponentInParent<GoldPickup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerModel playerModel = other.GetComponentInParent<PlayerModel>();

        if (playerModel == null)
            return;

        goldPickup.Collect(playerModel);
    }
}