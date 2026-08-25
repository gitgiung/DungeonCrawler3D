using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBG;
    [SerializeField] private Image hpImg;

    private PlayerModel model;
    private PlayerData data;
    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateHP();
    }

    // ***UI***

    private void UpdatePosition()
    {
        Vector3 pos =
            Camera.main.WorldToScreenPoint(
                transform.position
            );

        pos.y -= 8f;

        hpBG.transform.position = pos;
    }

    private void UpdateHP()
    {
        hpImg.fillAmount =
            (float)model.CurrentHP /
            data.MaxHP;
    }

    // ***Animation***
    
    public void PlayIdle()
    {
        animator.Play("S&S_Idle");
    }

    public void PlayMove()
    {
        animator.Play("S&S_Run");
    }

    public void PlaySprint()
    {
        animator.Play("S&S_ShieldRushLoop");
    }

    public void PlayAttack()
    {
        animator.Play("S&S_SwordAttack1");
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("S&S_SwordAttack1"))
            return false;

        return stateInfo.normalizedTime >= 1f;
    }
}
