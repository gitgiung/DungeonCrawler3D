using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerData data;

    public bool IsGround { get; private set; }

    private Rigidbody rb;

    private PlayerModel model;

    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;
    }

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
        model.GroundCheck.position,
        model.GroundCheckRadius,
        model.GroundLayer);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * data.JumpForce, ForceMode.Impulse);
    }
}
