using UnityEngine;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    private Animator animator;
    private Camera mainCamera;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Transform uiCanvas;

    [SerializeField] private Vector3 hpOffset = new Vector3(0f, 2f, 0f);

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
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    // *** UI ***
    private void UpdatePosition()
    {
        if (hpBarInstance == null || mainCamera == null)
            return;

        Vector3 worldPosition = transform.position + hpOffset;

        Vector3 viewportPos =
            mainCamera.WorldToViewportPoint(worldPosition);

        bool isVisible =
            viewportPos.z > 0f &&
            viewportPos.x >= 0f &&
            viewportPos.x <= 1f &&
            viewportPos.y >= 0f &&
            viewportPos.y <= 1f;

        hpBarInstance.SetActive(isVisible);

        if (!isVisible)
            return;

        Vector3 screenPos =
            mainCamera.WorldToScreenPoint(worldPosition);

        hpBarInstance.transform.position = screenPos;
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
        if (hpImg == null || monster == null)
            return;

        hpImg.fillAmount =
            (float)monster.Model.CurrentHP /
            monster.Data.MaxHP;
    }


// *** Animation ***
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
