using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 아이템 사용 목적
//  1. 소비 아이템
//  2. 장비 아이템
// 아이템 사용 여부를 Inventory에게 알려줌
// 아이템 생성 목적
//  1. 새로 생성
//  2. 기존 아이템 개수 추가
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private Image equipImg;
    [SerializeField] private TMP_Text countTxt;

    private int count;
    public int Count => count;

    private ItemScriptable data;
    public ItemScriptable Data => data; // 공유 변수

    private Inventory inventory;

    public void Init(ItemScriptable data, Inventory inventory)
    {
        this.data = data;
        this.inventory = inventory;
    }

    public void Setting()
    {
        iconImg.sprite = data.Icon;
        nameTxt.text = data.ItemName;
        equipImg.gameObject.SetActive(false);
        SetCount(1);

        if (data.ItemType == ItemType.Equipment)
            countTxt.gameObject.SetActive(false);
    }

    public void SetCount(int cnt)
    {
        count += cnt;
        countTxt.text = $"{count}";
    }
    
    // 아이템 클릭
    public void OnUse()
    {
        switch (data.ItemType)
        {
            case ItemType.Consumable:
                UseConsumable();
                break;
            case ItemType.Equipment:
                UseEquipment();
                break;
        }
    }

    // - 버튼 클릭
    public void OnDelete()
    {
        Debug.Log($"{data.ItemName} 삭제");
        inventory.DeleteItem(this);
    }

    private void UseConsumable()
    {
        Debug.Log($"소비 아이템 {data.ItemName} 사용");
        SetCount(-1);

        if (count <= 0)
        {
            inventory.DeleteItem(this);
        }
    }

    private void UseEquipment()
    {
        Debug.Log($"장비 아이템 {data.ItemName} 장착");
        equipImg.gameObject.SetActive(true);
    }
}
