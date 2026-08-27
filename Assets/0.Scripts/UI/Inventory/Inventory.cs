using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// 아이템 관리
// 아이템이 사용되었을 경우
// 아이템을 얻었을 경우
public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private InventoryItem invenItem;

    [SerializeField] private ItemScriptable[] itemDatas;

    private List<InventoryItem> items = new();

    private void Start()
    {
        itemDatas = Resources.LoadAll<ItemScriptable>("ItemData");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            int rand = Random.Range(0, itemDatas.Length);
            CreateItem(itemDatas[rand]);
        }
    }

    public void CreateItem(ItemScriptable data)
    {
        foreach (var i in items)
        {
            if (i.Data.ItemID == data.ItemID &&
                i.Count < data.MaxStack)
            {
                i.SetCount(1);
                return;
            }
        }
        InventoryItem creatItem = Instantiate(invenItem, parent);
        creatItem.Init(data, this);
        creatItem.Setting();
        items.Add(creatItem);
    }

    // 아이템을 다 소모하거나 삭제 버튼 클릭 시
    public void DeleteItem(InventoryItem item)
    {
        items.Remove(item);
        Destroy(item.gameObject);
    }
}
