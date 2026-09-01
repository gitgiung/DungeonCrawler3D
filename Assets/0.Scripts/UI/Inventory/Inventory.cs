using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            CreateItem(itemDatas[rand], 7);
        }
    }

    public int CreateItem(ItemScriptable data, int amount)
    {
        // 1. 기존 스택 중 여유가 있는 곳부터 채우기
        foreach (var item in items)
        {
            if (item.Data.ItemID != data.ItemID)
                continue;

            if (item.Count >= data.MaxStack)
                continue;

            int space = (int)data.MaxStack - item.Count;
            int addAmount = Mathf.Min(space, amount);

            item.AddCount(addAmount);
            amount -= addAmount;

            // 전부 넣었으면 종료
            if (amount <= 0)
                return 0;
        }

        // 2. 그래도 수량이 남아있다면 새로운 스택 생성
        while (amount > 0)
        {
            int stackAmount = Mathf.Min((int)data.MaxStack, amount);

            InventoryItem createItem = Instantiate(invenItem, parent);
            createItem.Init(data, this);
            createItem.Setting();

            // 기본 Count가 0이라면 그대로 사용
            createItem.AddCount(stackAmount);

            items.Add(createItem);

            amount -= stackAmount;
        }

        return 0;
    }

    // 아이템을 다 소모하거나 삭제 버튼 클릭 시
    public void DeleteItem(InventoryItem item)
    {
        items.Remove(item);
        Destroy(item.gameObject);
    }
}
