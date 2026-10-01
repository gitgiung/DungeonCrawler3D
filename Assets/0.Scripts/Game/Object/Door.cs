using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public string GetInteractionText()
    {
        return "¹® ¿­±â";
    }

    public void Interact()
    {
        animator.SetTrigger("door_open");

        Invoke("DoorClose", 3f);
    }

    void DoorClose()
    {
        animator.SetTrigger("door_close");
    }
}
