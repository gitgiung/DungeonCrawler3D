using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Header("IsGround")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    public bool IsGround { get; private set; }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        IsGround = Physics.CheckSphere(
        groundCheck.position,
        groundCheckRadius,
        groundLayer);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
