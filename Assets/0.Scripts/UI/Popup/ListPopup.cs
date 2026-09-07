using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ListPopup : MonoBehaviour
{
    [SerializeField] private List<Image> listImages = new List<Image>();

    private int showIndex = 0;

    public void PopupText(string text)
    {
        int index = showIndex;

        listImages[index].color = new Color(0, 0, 0, 0.6f);
        listImages[index].gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            listImages[index]
            .rectTransform
            .DOAnchorPos(new Vector2(0f, -25f), 1f)
            );

        sequence.Append(
            listImages[index]
            .DOFade(0f, 0.2f)
            .OnComplete(() =>
            {
                listImages[index]
                .rectTransform.position = new Vector3(0f, -225f, 0f);
                listImages[index].gameObject.SetActive(false);
            })
            );

        showIndex++;

        if (showIndex >= listImages.Count)
        {
            showIndex = 0;
        }
    }
}
