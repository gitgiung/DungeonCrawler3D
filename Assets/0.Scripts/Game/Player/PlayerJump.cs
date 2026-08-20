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
    private CapsuleCollider myCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = rb.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            Jump();
        }
    }

    private void CheckGround()
    {
        IsGround = Physics.CheckSphere(
        groundCheck.position,
        groundCheckRadius,
        groundLayer);

        //Debug.DrawRay(
        //groundCheck.position,
        //Vector3.down * groundCheckRadius,
        //isGround ? Color.green : Color.red
        //);
    }


    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
