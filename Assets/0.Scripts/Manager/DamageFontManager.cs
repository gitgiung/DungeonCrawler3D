using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageFontManager : Singleton<DamageFontManager>
{
    [SerializeField] private TMP_Text damageTxt;

    [Header("Damage Text")]
    [SerializeField] private Vector3 txtOffset = new (0f, 1.5f, 0f);
    [SerializeField] private float moveDistance = 80f;
    [SerializeField] private float duration = 0.8f;

    public void CreateText(int damage, Vector3 pos)
    {
        // 몬스터 위치보다 위쪽에서 생성
        Vector3 spawnWorldPos = pos + txtOffset;

        // 월드 좌표 → 화면 좌표
        Vector3 uiPos = Camera.main.WorldToScreenPoint(spawnWorldPos);

        TMP_Text txt = Instantiate(
            damageTxt,
            uiPos,
            Quaternion.identity,
            transform
        );

        txt.text = $"{damage}";

        float startY = txt.rectTransform.position.y;

        //프리팹의 알파값이 0으로 남아있을 경우
        txt.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        // 위로 이동
        seq.Append(
            txt.rectTransform
                .DOMoveY(startY + moveDistance, duration)
                .SetEase(Ease.OutQuad)
        );

        // 위로 이동하면서 동시에 투명해짐
        seq.Join(
            txt.DOFade(0f, duration)
        );

        // 애니메이션 종료 후 제거
        seq.OnComplete(() =>
        {
            Destroy(txt.gameObject);
        });
    }
}