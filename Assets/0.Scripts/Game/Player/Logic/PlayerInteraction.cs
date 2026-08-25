using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact UI")]
    [SerializeField] private Image UI_F;

    [SerializeField, Range(1f, 5f)]
    private float interactRadius = 1f;

    [Header("Gizmos")]
    [SerializeField] Color gizmosColor = Color.yellow;
    [SerializeField] private float posY = 1f;

    private Vector3 pos;

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        CheckInteractable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        pos = transform.position;
        pos.y += posY;
        Gizmos.DrawWireSphere(pos,
                interactRadius);
    }

    private void CheckInteractable()
    {
        pos = transform.position;
        pos.y += posY;

        Collider[] colliders =
            Physics.OverlapSphere(
                pos,
                interactRadius
            );

        bool foundInteractable = false;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(
                out IInteractable interact))
            {
                foundInteractable = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    interact.Interact();
                }

                break;
            }
        }

        UI_F.gameObject.SetActive(foundInteractable);
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 pos =
            Camera.main.WorldToScreenPoint(
                transform.position);
        pos.y += 35f;

        UI_F.transform.position = pos;
    }
}