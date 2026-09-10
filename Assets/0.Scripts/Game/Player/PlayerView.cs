using TMPro;
using UnityEngine;
using UnityEngine.UI;

// View: Model에서 관리하는 데이터를 읽고 사용자에게 표시
public class PlayerView : MonoBehaviour
{
    // Animator에서 문자열로 재생 시 어차피 내부적으로 해시 변환하기 때문에 미리 캐싱
    private static readonly int IdleStateHash = Animator.StringToHash("S&S_Idle");
    private static readonly int MoveStateHash = Animator.StringToHash("S&S_Run");
    private static readonly int SprintStateHash = Animator.StringToHash("S&S_ShieldRushLoop");
    private static readonly int AttackStateHash = Animator.StringToHash("S&S_SwordAttack1");
    private static readonly int HitStateHash = Animator.StringToHash("S&S_Hit");
    private static readonly int DeadStateHash = Animator.StringToHash("S&S_Death");

    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private Image hpImg;
    [SerializeField] private TMP_Text currentHpText;
    [SerializeField] private TMP_Text maxHpText;

    [Header("EXP Bar")]
    [SerializeField] private Image expImg;

    [Header("Gold")]
    [SerializeField] private TMP_Text currentGold;

    [Header("Level")]
    [SerializeField] private TMP_Text currentLevel;

    private PlayerModel model;
    public void Init(PlayerModel model)
    {
        this.model = model;

        model.OnHPChanged += UpdateHP;
        model.OnGoldChanged += UpdateGold;
        model.OnExpChanged += UpdateExp;
        model.OnLevelChanged += UpdateLevel;
        model.OnStatsChanged += UpdateStats;

        UpdateHP(model.CurrentHP, model.MaxHP);
        UpdateGold(model.Gold);
        UpdateExp(model.Exp, model.MaxExp);
        UpdateLevel(model.Level);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ***UI***
    private void UpdateHP(int currentHp, int maxHp)
    {
        hpImg.fillAmount = (float)currentHp / maxHp;
        currentHpText.text = $"{currentHp}";
        maxHpText.text = $"{maxHp}";
    }

    private void UpdateExp(int exp, int maxExp)
    {
        expImg.fillAmount = (float)exp / maxExp;
    }

    private void UpdateGold(int gold)
    {
        currentGold.text = $"{gold}";
    }

    private void UpdateLevel(int level)
    {
        currentLevel.text = $"{level}";
    }

    private void UpdateStats()
    {
        UpdateHP(model.CurrentHP, model.MaxHP);
        UpdateExp(model.Exp, model.MaxExp);
    }

    private void OnDestroy()
    {
        model.OnHPChanged -= UpdateHP;
        model.OnGoldChanged -= UpdateGold;
        model.OnExpChanged -= UpdateExp;
        model.OnLevelChanged -= UpdateLevel;
        model.OnStatsChanged -= UpdateStats;
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
