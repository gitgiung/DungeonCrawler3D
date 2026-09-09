using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject txtBGObj;
    [SerializeField] private Image iconImg;
    [SerializeField] private TMP_Text itemNameTxt;

    [SerializeField] private ItemScriptable data;
    public ItemScriptable Data => data;

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

        this.data = data;

        iconImg.sprite = this.data.Icon;
        itemNameTxt.text = this.data.ItemName;

        iconImg.gameObject.SetActive(true);
        txtBGObj.SetActive(true);

        RefreshEquipmentStats();
    }

    public void UnEquip()
    {
        data = null;

        iconImg.sprite = null;
        itemNameTxt.text = string.Empty;
        iconImg.gameObject.SetActive(false);
        txtBGObj.SetActive(false);

        RefreshEquipmentStats();
    }

    private void RefreshEquipmentStats()
    {
        EquipmentSystem equipmentSystem = EquipmentSystem.Instance;

        if (equipmentSystem != null)
            equipmentSystem.RefreshStats();
    }
}
