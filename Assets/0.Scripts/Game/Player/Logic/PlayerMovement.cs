using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 movement;

    private Vector3 lastMoveDirection = Vector3.right;
    public Vector3 LastMoveDirection => lastMoveDirection;

    private Rigidbody rb;

    private PlayerModel model;

    public void Initialize(PlayerModel model)
    {
        this.model = model;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        moveSpeed = model.WalkSpeed;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        Move();
    }

    private bool canMove;

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetMovement(Vector3 direction)
    {
        movement = direction;
    }

    public void SetSprint(bool sprint)
    {
        moveSpeed = sprint ? model.SprintSpeed : model.WalkSpeed;
    }

    public void Move()
    {
        Vector3 direction = movement.normalized;

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void Stop()
    {
        movement = Vector3.zero;
    }

    public void Look()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0f;

            lastMoveDirection = dir.normalized;

            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}
