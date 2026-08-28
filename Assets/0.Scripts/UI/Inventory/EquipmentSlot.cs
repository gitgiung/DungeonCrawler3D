using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject txtBGObj;
    [SerializeField] private Image iconImg;
    [SerializeField] private TMP_Text itemNameTxt;

    private MoveItem moveItem;
    public ItemScriptable Data { get; set; }

    public void Start()
    {
        iconImg.gameObject.SetActive(false);
        txtBGObj.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIController.Instance.equipmentSystem.SelectSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIController.Instance.equipmentSystem.SelectSlot = null;
    }

    public void Equip(ItemScriptable data)
    {
        if (data == null)
            return;

        if (data.ItemType != ItemType.Equipment)
            return;

        iconImg.sprite = data.Icon;
        itemNameTxt.text = data.ItemName;

        iconImg.gameObject.SetActive(true);
        txtBGObj.SetActive(true);
    }

    public void UnEquip()
    {
        iconImg.gameObject.SetActive(false);
        txtBGObj.SetActive(false);
    }
}
