using DG.Tweening;
using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    [Header("µÂ∑” ø¨√‚")]
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 0.6f;
    [SerializeField] private float dropRadius = 0.7f;
    [SerializeField] private float landingHeight = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("µ’µ’ ¿Ãµø")]
    [SerializeField] private float floatingHeight = 0.25f;
    [SerializeField] private float floatingDuration = 1.2f;

    [Header("≈©±‚ ∏∆µø")]
    [SerializeField] private Transform visual;
    [SerializeField] private float pulseScale = 1.08f;
    [SerializeField] private float pulseDuration = 0.8f;

    private Tween floatingTween;
    private Tween pulseTween;

    private void Start()
    {
        if (visual == null)
            visual = transform;

        Drop();
    }

    private void Drop()
    {
        Vector3 landingPosition = GetLandingPosition();

        transform
            .DOJump(
                landingPosition,
                jumpPower,
                1,
                jumpDuration
            )
            .SetEase(Ease.OutQuad)
            .OnComplete(StartFloating);
    }

    private Vector3 GetLandingPosition()
    {
        Vector2 random = Random.insideUnitCircle * dropRadius;

        Vector3 rayOrigin = new Vector3(
            transform.position.x + random.x,
            transform.position.y + 2f,
            transform.position.z + random.y
        );

        if (Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                10f,
                groundLayer))
        {
            return hit.point + Vector3.up * landingHeight;
        }

        return transform.position;
    }

    private void StartFloating()
    {
        StartFloatMovement();
        StartPulse();
    }

    private void StartFloatMovement()
    {
        floatingTween = transform
            .DOMoveY(
                transform.position.y + floatingHeight,
                floatingDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StartPulse()
    {
        Vector3 targetScale = visual.localScale * pulseScale;

        pulseTween = visual
            .DOScale(targetScale, pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        floatingTween?.Kill();
        pulseTween?.Kill();

        transform.DOKill();
        visual.DOKill();
    }
}