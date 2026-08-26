using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Transform uiCanvas;
    private Image hpImg;
    private GameObject hpBarInstance;

    private PlayerModel model;
    private PlayerData data;
    public void Initialize(PlayerModel model, PlayerData data)
    {
        this.model = model;
        this.data = data;

        CreateHPBar();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdatePosition();
    }

    // ***UI***
    private void UpdatePosition()
    {
        Vector3 pos =
            Camera.main.WorldToScreenPoint(
                transform.position
            );

        pos.y -= 10f;

        hpBarInstance.transform.position = pos;
    }

    private void CreateHPBar()
    {
        hpBarInstance = Instantiate(hpBar, uiCanvas);

        hpImg = hpBarInstance.transform
            .Find("CurrentHP")
            .GetComponent<Image>();

        UpdateHP();
    }

    public void UpdateHP()
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
