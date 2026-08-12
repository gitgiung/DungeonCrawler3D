using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    private Vector3 lastLookDirection = Vector3.right;

    [Header("Player Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float moveSpeed;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] GameObject dashShadow;

    private bool isGround;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody rb;
    private CapsuleCollider myCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (movement != Vector3.zero)
        {
            lastLookDirection = movement;
        }

        Move();
        Sprint();
        Jump();
        Look();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine("Dash");
        }

    }

    private void FixedUpdate()
    {
        IsGround();
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
                //transform.forward = dir;
            }
        }
    }

    void OnMove(InputValue inputValue)
    {
        //WASD, Arrow
        Vector2 vec = inputValue.Get<Vector2>();
        movement = new Vector3(vec.x, 0, vec.y);
    }

    void Move()
    {
        movement.Normalize();
        //transform.position += movement * Time.deltaTime * moveSpeed;
        transform.Translate(movement * Time.deltaTime * moveSpeed, Space.World);
    }


    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    public IEnumerator Dash()
    {
        Vector3 dashDir = lastLookDirection;

        float timer = 0f;
        while (timer < dashDuration)
        {
            transform.position += dashDir * dashSpeed * Time.deltaTime;
            timer += Time.deltaTime;

            GameObject obj = Instantiate(dashShadow, transform.position, transform.rotation);
            yield return new WaitForFixedUpdate();
            Destroy(obj, 0.2f);
        }
    }


    private void Jump()
    {
        //Space
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        //Left Click
        Debug.Log("АјАн");
    }

}
