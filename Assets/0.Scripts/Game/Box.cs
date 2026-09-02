using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "상자 열기";
    }

    public void Interact()
    {
        Debug.Log("보물 상자");
    }
}
