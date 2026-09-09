using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private static readonly int IdleStateHash = Animator.StringToHash("S&S_Idle");
    private static readonly int MoveStateHash = Animator.StringToHash("S&S_Run");
    private static readonly int SprintStateHash = Animator.StringToHash("S&S_ShieldRushLoop");
    private static readonly int AttackStateHash = Animator.StringToHash("S&S_SwordAttack1");
    private static readonly int HitStateHash = Animator.StringToHash("S&S_Hit");
    private static readonly int DeadStateHash = Animator.StringToHash("S&S_Death");

    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private Image hpImg;

    [Header("EXP Bar")]
    [SerializeField] private Image expImg;

    [Header("Gold")]
    [SerializeField] private TMP_Text currentGold;

    private PlayerModel model;
    public void Init(PlayerModel model)
    {
        this.model = model;

        model.OnHPChanged += UpdateHP;
        model.OnGoldChanged += UpdateGold;
        model.OnExpChanged += UpdateExp;

        UpdateHP(model.CurrentHP);
        UpdateGold(model.Gold);
        UpdateExp(model.Exp);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ***UI***
    public void UpdateHP(int currentHp)
    {
        hpImg.fillAmount = (float)currentHp / model.MaxHP;
    }

    public void UpdateExp(int exp)
    {
        expImg.fillAmount = (float)exp / model.MaxExp;
    }

    public void UpdateGold(int gold)
    {
        currentGold.text = $"{model.Gold}";
    }

    // ***Animation***
    public void PlayIdle()
    {
        animator.Play(IdleStateHash);
    }

    public void PlayMove()
    {
        animator.Play(MoveStateHash);
    }

    public void PlaySprint()
    {
        animator.Play(SprintStateHash);
    }

    public void PlayAttack()
    {
        animator.Play(AttackStateHash);
    }

    public void PlayHit()
    {
        animator.Play(HitStateHash);
    }

    public void PlayDead()
    {
        animator.Play(DeadStateHash);
    }

    public bool IsAttackAnimationFinished()
    {
        return IsAnimationFinished(AttackStateHash);
    }

    public bool IsHitAnimationFinished()
    {
        return IsAnimationFinished(HitStateHash);
    }

    private bool IsAnimationFinished(int stateHash)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.shortNameHash == stateHash &&
               stateInfo.normalizedTime >= 1f;
    }
}
