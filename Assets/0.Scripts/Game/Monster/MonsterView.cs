using UnityEngine;

public class MonsterView : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        animator.Play("Idle");
    }

    public void PlayChase()
    {
        animator.Play("Run");
    }

    public void PlayPatrol()
    {
        animator.Play("Walk");
    }

    public void PlayAttack()
    {
        animator.Play("Jump", 0, 0f);
    }

    public void PlayHit()
    {
        animator.Play("TPose");
    }

    public void PlayDeath()
    {
        animator.Play("Loose");
    }
}
