using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    private PickupEffect pickupEffect;
    private Collider triggerCollider;

    private bool isPickedUp;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        pickupEffect = GetComponentInParent<PickupEffect>();

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning(
                $"{name}의 Collider는 Is Trigger가 활성화되어 있어야 합니다.",
                gameObject);
        }

        if (pickupEffect == null)
        {
            Debug.LogError(
                $"{name}의 부모 오브젝트에서 PickupEffect를 찾을 수 없습니다.",
                gameObject);

            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp)
        {
            Debug.Log("isPickedUp이 문제야");
            return;
        }

        if (!other.CompareTag("Player"))
            return;

        PlayerModel playerModel = other.GetComponentInParent<PlayerModel>();

        if (playerModel == null)
        {
            Debug.Log("모델이 없어");
            return;
        }
            

        if (!pickupEffect.Apply(playerModel))
        {
            Debug.Log("Apply 작동 안함");
            return;
        }

        isPickedUp = true;

        // 중복 획득 방지
        triggerCollider.enabled = false;

        // PickupEffect가 붙어 있는 루트 프리팹 제거
        Destroy(pickupEffect.gameObject);
    }
}