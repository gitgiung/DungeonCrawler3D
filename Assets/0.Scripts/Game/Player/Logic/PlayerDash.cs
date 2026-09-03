using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private const float ShadowInterval = 0.02f;
    private const float ShadowLifetime = 0.2f;

    private PlayerData data;
    private PlayerModel model;
    private Vector3 dashDirection;
    private float remainingTime;
    private float shadowTimer;

    public bool IsDashing { get; private set; }
    public Vector3 Velocity => IsDashing
        ? dashDirection * data.DashSpeed
        : Vector3.zero;

    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;
    }

    public void StartDash(Vector3 direction)
    {
        if (data.DashDuration <= 0f)
        {
            StopDash();
            return;
        }

        dashDirection = direction.sqrMagnitude > 0.001f
            ? direction.normalized
            : Vector3.right;

        remainingTime = data.DashDuration;
        shadowTimer = ShadowInterval;
        IsDashing = true;
        CreateShadow();
    }

    public void Tick(float deltaTime)
    {
        if (!IsDashing)
            return;

        remainingTime -= deltaTime;
        shadowTimer -= deltaTime;

        if (shadowTimer <= 0f)
        {
            CreateShadow();
            shadowTimer += ShadowInterval;
        }

        if (remainingTime <= 0f)
            StopDash();
    }

    public void StopDash()
    {
        IsDashing = false;
        remainingTime = 0f;
    }

    private void CreateShadow()
    {
        if (model.DashShadow == null)
            return;

        GameObject shadow = Instantiate(
            model.DashShadow,
            transform.position,
            transform.rotation
        );

        Destroy(shadow, ShadowLifetime);
    }
}
