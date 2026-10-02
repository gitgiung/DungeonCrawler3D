using DG.Tweening;
using UnityEngine;

public class GoldDropMotion : MonoBehaviour
{
    [Header("드롭 연출")]
    [SerializeField]
    [Tooltip("아이템이 튀어오르는 높이입니다.")]
    private float jumpPower = 1.5f;

    [SerializeField]
    [Tooltip("아이템이 떨어지는 데 걸리는 시간입니다.")]
    private float jumpDuration = 0.6f;

    private Rigidbody rb;
    private Tween dropTween;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Play(Vector3 targetPosition)
    {
        // DOTween이 Transform을 움직이는 동안
        // Rigidbody 물리 이동과 충돌하지 않도록 한다.
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        dropTween = transform
            .DOJump(
                targetPosition,
                jumpPower,
                1,
                jumpDuration
            )
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (rb != null)
                    rb.isKinematic = false;
            });
    }

    private void OnDestroy()
    {
        dropTween?.Kill();
    }
}