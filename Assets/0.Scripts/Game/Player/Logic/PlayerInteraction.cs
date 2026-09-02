using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact UI")]
    [SerializeField] private Image UI_F;
    [SerializeField] private TMP_Text interactionText;

    [SerializeField, Range(1f, 5f)]
    private float interactRadius = 1f;

    [Header("Gizmos")]
    [SerializeField] Color gizmosColor = Color.yellow;
    [SerializeField] private float posY = 1f;

    private Vector3 checkPosition;

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            HideInteractionUI();
            return;
        }

        CheckInteractable();
    }

    private void CheckInteractable()
    {
        checkPosition = transform.position;
        checkPosition.y += posY;

        Collider[] colliders = Physics.OverlapSphere(
            checkPosition,
            interactRadius
        );

        IInteractable nearestInteractable = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            IInteractable interactable =
                collider.GetComponentInParent<IInteractable>();

            if (interactable == null)
                continue;

            Vector3 closestPoint = collider.ClosestPoint(checkPosition);
            float distance =
                (closestPoint - checkPosition).sqrMagnitude;

            if (distance >= nearestDistance)
                continue;

            nearestDistance = distance;
            nearestInteractable = interactable;
        }

        if (nearestInteractable == null)
        {
            HideInteractionUI();
            return;
        }

        ShowInteractionUI(nearestInteractable.GetInteractionText());

        if (Input.GetKeyDown(KeyCode.F))
        {
            // 대화창 뒤에 F 안내가 남지 않게 먼저 숨긴다.
            HideInteractionUI();
            nearestInteractable.Interact();
        }
    }

    private void ShowInteractionUI(string message)
    {
        UI_F.gameObject.SetActive(true);
        interactionText.text = message;
        UpdatePosition();
    }

    private void HideInteractionUI()
    {
        UI_F.gameObject.SetActive(false);
        interactionText.text = string.Empty;
    }

    private void UpdatePosition()
    {
        Vector3 pos =
            Camera.main.WorldToScreenPoint(
                transform.position);
        pos.y += 35f;

        UI_F.transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        checkPosition = transform.position;
        checkPosition.y += posY;
        Gizmos.DrawWireSphere(checkPosition,
                interactRadius);
    }
}