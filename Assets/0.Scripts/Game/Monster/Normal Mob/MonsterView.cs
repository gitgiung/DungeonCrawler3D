using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    private static readonly int AttackStateHash = Animator.StringToHash("Jump");

    // ���� �ǰݽ� shader ����
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private Vector3 hpOffset = new Vector3(0f, 2f, 0f);

    [Header("Damage Flash")]
    [SerializeField] private Color damageFlashColor = Color.red;

    [SerializeField, Min(0.01f)]
    private float damageFlashDuration = 0.1f;

    // Damage Flash
    private Renderer[] renderers;
    private MaterialPropertyBlock[] originalPropertyBlocks;
    private MaterialPropertyBlock[] flashPropertyBlocks;
    private Coroutine damageFlashCoroutine;

    private Image hpImg;
    private GameObject hpBarInstance;

    private Animator animator;
    private Camera mainCamera;

    private Monster monster;
    private MonsterModel model;

    public void Initialize(Monster monster, MonsterModel model)
    {
        this.monster = monster;
        this.model = model;

        CreateHPBar();

        model.OnHPChange += UpdateHP;
        UpdateHP(model.CurrentHP);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        CacheRenderers();
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

        // 카메라에 보일 때만 HP Bar 노출
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
    }

    public void UpdateHP(int currentHP)
    {
        if (hpImg == null || monster == null)
            return;

        hpImg.fillAmount =
            (float)currentHP /
            monster.Data.MaxHP;
    }

    public void HideHPBar()
    {
        if (hpBarInstance != null)
            hpBarInstance.SetActive(false);
    }

    private void OnDestroy()
    {
        if (hpBarInstance != null)
            Destroy(hpBarInstance);
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
        animator.Play("TPose", 0, 0f);
    }

    public void PlayDeath()
    {
        animator.Play("Loose", 0, 0f);
    }

    public bool IsAttackAnimationFinished()
    {
        if (animator == null || !animator.enabled)
            return true;

        AnimatorStateInfo stateInfo =
            animator.GetCurrentAnimatorStateInfo(0);

        bool isAttackState =
            stateInfo.shortNameHash == AttackStateHash;

        if (!isAttackState)
            return false;

        return stateInfo.normalizedTime >= 1f &&
               !animator.IsInTransition(0);
    }

    // *** Damage Flash ***
    // 피격 시 적색으로 점멸하도록 한다
    // 시각적 피드백 받기 위해 GPT 코드 사용

    private void CacheRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>(true);

        originalPropertyBlocks =
            new MaterialPropertyBlock[renderers.Length];

        flashPropertyBlocks =
            new MaterialPropertyBlock[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalPropertyBlocks[i] =
                new MaterialPropertyBlock();

            flashPropertyBlocks[i] =
                new MaterialPropertyBlock();

            renderers[i].GetPropertyBlock(
                originalPropertyBlocks[i]
            );

            renderers[i].GetPropertyBlock(
                flashPropertyBlocks[i]
            );

            // URP Lit Shader
            flashPropertyBlocks[i].SetColor(
                BaseColorId,
                damageFlashColor
            );

            // Standard Shader
            flashPropertyBlocks[i].SetColor(
                ColorId,
                damageFlashColor
            );
        }
    }

    public void PlayDamageFlash()
    {
        if (!isActiveAndEnabled || renderers.Length == 0)
            return;

        // ���� �ǰ� �� ���� ������ �ߴ��ϰ� ���� ������ �ǵ�����.
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            RestoreRendererColors();
        }

        damageFlashCoroutine =
            StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        ApplyFlashColor();

        yield return new WaitForSeconds(damageFlashDuration);

        RestoreRendererColors();

        damageFlashCoroutine = null;
    }

    private void ApplyFlashColor()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;

            renderers[i].SetPropertyBlock(
                flashPropertyBlocks[i]
            );
        }
    }

    private void RestoreRendererColors()
    {
        if (renderers == null ||
            originalPropertyBlocks == null)
        {
            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;

            renderers[i].SetPropertyBlock(
                originalPropertyBlocks[i]
            );
        }
    }
}
