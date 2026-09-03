using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerDash))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerData data;
    private CharacterController characterController;
    private PlayerJump jump;
    private PlayerDash dash;
    private float moveSpeed;
    private Vector3 desiredDirection;
    private Vector3 lastLookDirection = Vector3.right;
    private bool canMove;

    public Vector3 LastLookDirection => lastLookDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        jump = GetComponent<PlayerJump>();
        dash = GetComponent<PlayerDash>();
    }

    public void Initialize(PlayerData data)
    {
        this.data = data;
        moveSpeed = data.WalkSpeed;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetMovement(Vector3 direction)
    {
        desiredDirection = direction.sqrMagnitude > 0.001f
            ? direction.normalized
            : Vector3.zero;
    }

    public void SetSprint(bool sprint)
    {
        moveSpeed = sprint ? data.SprintSpeed : data.WalkSpeed;
    }

    public void Tick()
    {
        if (!enabled || !characterController.enabled)
            return;

        float deltaTime = Time.deltaTime;
        bool controllerGrounded = characterController.isGrounded;

        jump.TickGravity(deltaTime, controllerGrounded);
        bool wasDashing = dash.IsDashing;

        bool airborne = jump.VerticalVelocity > 0f ||
                        (!controllerGrounded && !jump.IsGround);

        Vector3 horizontalVelocity;

        if (wasDashing)
        {
            horizontalVelocity = dash.Velocity;
        }
        else if (airborne)
        {
            horizontalVelocity = canMove
                ? GetGroundVelocity()
                : Vector3.zero;

            if (horizontalVelocity.sqrMagnitude > 0.001f)
                FaceDirection(horizontalVelocity, deltaTime);
        }
        else if (canMove)
        {
            horizontalVelocity = GetGroundVelocity();

            if (horizontalVelocity.sqrMagnitude > 0.001f)
                FaceDirection(horizontalVelocity, deltaTime);
        }
        else
        {
            horizontalVelocity = Vector3.zero;
        }

        Vector3 velocity = horizontalVelocity +
                           Vector3.up * jump.VerticalVelocity;

        CollisionFlags flags = characterController.Move(
            velocity * deltaTime
        );

        bool grounded = (flags & CollisionFlags.Below) != 0 ||
                        characterController.isGrounded;

        if ((controllerGrounded || jump.IsGround) &&
            !grounded &&
            jump.VerticalVelocity <= 0f)
        {
            jump.BeginFall();
        }

        jump.SetGrounded(grounded);

        if (wasDashing)
            dash.Tick(deltaTime);

        if (wasDashing &&
            (flags & CollisionFlags.Sides) != 0)
        {
            dash.StopDash();
        }
    }

    private Vector3 GetGroundVelocity()
    {
        return desiredDirection * moveSpeed;
    }

    private void FaceDirection(Vector3 direction, float deltaTime)
    {
        Vector3 lookDirection = direction.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            data.RotationSpeed * deltaTime
        );

        lastLookDirection = lookDirection;
    }
}
