using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private GameObject dashShadow;

    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartDash();
        }
    }

    public void StartDash()
    {
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        Vector3 dashDir = movement.LastMoveDirection;

        float timer = 0f;

        while (timer < dashDuration)
        {
            transform.position +=
                dashDir * dashSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            GameObject obj = Instantiate(
                dashShadow,
                transform.position,
                transform.rotation
            );

            yield return new WaitForFixedUpdate();

            Destroy(obj, 0.2f);
        }
    }
}