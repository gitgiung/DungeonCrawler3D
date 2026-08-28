using UnityEngine;
using UnityEngine.UI;

public class MoveItem : MonoBehaviour
{
    [SerializeField] private Image iconImg;

    private RectTransform rectTransform;

    public ItemScriptable Data { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // MoveItem이 마우스 Raycast를 가로막지 않게 함
        iconImg.raycastTarget = false;

        Hide();
    }

    public void Show(ItemScriptable data, Vector2 position)
    {
        Data = data;

        iconImg.sprite = data.Icon;
        rectTransform.position = position;

        gameObject.SetActive(true);
    }

    public void Move(Vector2 position)
    {
        rectTransform.position = position;
    }

    public void Hide()
    {
        Data = null;
        gameObject.SetActive(false);
    }
}