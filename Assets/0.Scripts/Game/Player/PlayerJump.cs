using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    private bool isGround;

    private Rigidbody rb;
    private CapsuleCollider myCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = rb.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        IsGround();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        Jump();
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    private void Jump()
    {
        //Space
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
