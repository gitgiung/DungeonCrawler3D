using DG.Tweening;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    private int amount;
    private bool collected;

    [Header("Drop Animation")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float jumpDuration = 0.35f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int amount)
    {
        this.amount = amount;

        PlayDropAnimation();
    }

    private void PlayDropAnimation()
    {
        rb.isKinematic = true;

        Vector3 targetPosition = transform.position + new Vector3(
            Random.Range(-0.5f, 0.5f),
            jumpHeight,
            Random.Range(-0.5f, 0.5f)
        );

        transform
            .DOMove(targetPosition, jumpDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rb.isKinematic = false;
            });
    }

    public void Collect(PlayerModel playerModel)
    {
        if (collected)
            return;

        collected = true;

        playerModel.AddGold(amount);

        Destroy(gameObject);
    }
}