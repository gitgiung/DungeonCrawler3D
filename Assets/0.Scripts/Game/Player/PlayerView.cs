using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBar;
    private Image hpImg;

    [Header("EXP Bar")]
    [SerializeField] private GameObject expBar;
    private Image expImg;

    [Header("Gold")]
    [SerializeField] private TMP_Text currentGold;

    private PlayerModel model;
    private PlayerData data;
    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;

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
        hpImg = hpBar.transform.Find("CurrentHP").GetComponent<Image>();
        expImg = expBar.transform.Find("CurrentEXP").GetComponent <Image>();
    }

    // ***UI***

    public void UpdateHP(int currentHp)
    {
        hpImg.fillAmount = (float)currentHp / data.MaxHP;
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
