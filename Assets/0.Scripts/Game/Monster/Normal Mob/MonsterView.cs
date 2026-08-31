using UnityEngine;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    private Animator animator;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Transform uiCanvas;
    private Image hpImg;
    private GameObject hpBarInstance;
    
    private Monster monster;

    public void Initialize(Monster monster)
    {
        this.monster = monster;

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

        pos.y -= 8f;

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
            (float)monster.Model.CurrentHP /
            monster.Data.MaxHP;
    }


    // ***Animation***
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
