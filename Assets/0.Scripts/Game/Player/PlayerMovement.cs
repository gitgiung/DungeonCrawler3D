using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float moveSpeed;
    private Vector3 movement;

    private Vector2 vec;
    //이동 상태 체크
    public bool HasInput => vec.sqrMagnitude > 0.001f;
    public Vector3 Movement => movement;

    private Vector3 lastMoveDirection = Vector3.right;
    public Vector3 LastMoveDirection => lastMoveDirection;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        moveSpeed = walkSpeed;
    }

    void OnMove(InputValue inputValue)
    {
        //WASD, Arrow
        vec = inputValue.Get<Vector2>();
        movement = new Vector3(vec.x, 0, vec.y);
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (movement != Vector3.zero)
        {
            lastMoveDirection = movement.normalized;
        }

        Sprint();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector3 direction = movement.normalized;

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;
    }

    public void Look()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}
