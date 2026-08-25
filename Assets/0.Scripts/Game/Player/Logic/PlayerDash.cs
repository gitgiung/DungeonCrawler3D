using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    private PlayerData data;

    public bool IsDashing { get; private set; }
    private PlayerMovement movement;

    private PlayerModel model;

    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;
    }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void StartDash()
    {
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        IsDashing = true;
        Vector3 dashDir = movement.LastMoveDirection;

        float timer = 0f;

        while (timer < data.DashDuration)
        {
            transform.position +=
                data.DashSpeed * Time.deltaTime * dashDir;

            timer += Time.deltaTime;

            GameObject obj = Instantiate(
                model.DashShadow,
                transform.position,
                transform.rotation
            );

            yield return new WaitForFixedUpdate();

            Destroy(obj, 0.2f);
        }

        IsDashing = false;
    }
}