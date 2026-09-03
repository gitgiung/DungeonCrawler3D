using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private const float GroundedVelocity = -2f;

    private PlayerData data;

    public bool IsGround { get; private set; }
    public float VerticalVelocity { get; private set; }

    public void Initialize(PlayerData data)
    {
        this.data = data;
    }

    public void BeginJump()
    {
        VerticalVelocity = Mathf.Sqrt(data.JumpHeight * -2f * data.Gravity);
        IsGround = false;
    }

    public void BeginFall()
    {
        IsGround = false;
    }

    public void TickGravity(float deltaTime, bool controllerGrounded)
    {
        if (controllerGrounded && VerticalVelocity < 0f)
        {
            VerticalVelocity = GroundedVelocity;
            return;
        }

        VerticalVelocity += data.Gravity * deltaTime;
    }

    public void SetGrounded(bool grounded)
    {
        IsGround = grounded;

        if (!grounded || VerticalVelocity >= 0f)
            return;

        VerticalVelocity = GroundedVelocity;
    }
}
