using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageFontManager : Singleton<DamageFontManager>
{
    [SerializeField] TMP_Text damageTxt;

    public float jumpHeight = 2f;
    public float duration = 0.8f;

    public void CreateText(int damage, Vector3 pos)
    {
        Vector3 uiPos = Camera.main.WorldToScreenPoint(pos);
        TMP_Text txt = Instantiate(damageTxt, uiPos, Quaternion.identity, transform);
        txt.text = $"{damage}";

        Vector3 startPos = pos;

        Sequence seq = DOTween.Sequence();

        //위로 튀어오르기
        seq.Append
        (
            transform.DOMoveY(startPos.y + jumpHeight, duration * 0.45f).SetEase(Ease.OutQuad)
        );

        //다시 떨어지기
        seq.Append
        (
            txt.rectTransform.DOMoveY(startPos.y, duration * 0.55f).SetEase(Ease.InQuad)
        );

        //사라지기
        seq.Append
        (
            txt.rectTransform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack)
        );

        //애니메이션 끝나면 오브젝트 제거
        seq.OnComplete(() =>
        {
            Destroy(txt.gameObject);
        });
    }
}
