using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToastPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private HorizontalLayoutGroup hlg;

    private Queue<string> popupQueue = new Queue<string>();
    private bool isAnimation = false;

    public void PopupText(string text)
    {
        popupQueue.Enqueue(text);
        
    }

    private void Update()
    {
        if (popupQueue.Count != 0 && !isAnimation)
        {
            popupText.text = popupQueue.Dequeue();
            StartCoroutine("PopupAnimation");
        }
    }

    private IEnumerator PopupAnimation()
    {
        isAnimation = true;

        hlg.enabled = false;
        yield return new WaitForSeconds(0.2f);
        hlg.enabled = true;
        GetComponent<Image>()
            .rectTransform
            .DOAnchorPos(new Vector2(0f, -170f), 1f)
            .SetEase(Ease.OutBack)
            .SetUpdate(false)
            .OnComplete(() =>
            {
                GetComponent<Image>()
                .rectTransform
                .DOAnchorPos(new Vector2(0f, 0f), 0.5f)
                .SetDelay(2f)
                .SetEase(Ease.Linear)
                .SetUpdate(false)
                .OnComplete(() =>
                {
                    isAnimation = false;
                });
            });

        yield return null;
    }

}
