using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "대화하기";
    }

    public void Interact()
    {
        Debug.Log("NPC와 대화");
    }
}
