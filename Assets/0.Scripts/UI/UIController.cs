using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : Singleton<UIController>
{
    public Inventory inventory;
    public EquipmentSystem equipmentSystem;
    public MoveItem moveItem;
    public Canvas canvas;
    public Transform monsterUICanvas;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventory.gameObject.SetActive(!inventory.gameObject.activeInHierarchy);
            equipmentSystem.gameObject.SetActive(!equipmentSystem.gameObject.activeInHierarchy);
        }
    }
}
