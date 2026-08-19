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

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        CheckInteractable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position,
                interactRadius);
    }

    private void CheckInteractable()
    {
        Collider[] colliders =
            Physics.OverlapSphere(
                transform.position,
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
    }
}