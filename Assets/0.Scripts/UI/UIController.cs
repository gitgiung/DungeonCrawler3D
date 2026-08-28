using Unity.VisualScripting;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public Inventory inventory;
    public EquipmentSystem equipmentSystem;
    public MoveItem moveItem;
    public Canvas canvas;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventory.gameObject.SetActive(!inventory.gameObject.activeInHierarchy);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            equipmentSystem.gameObject.SetActive(!equipmentSystem.gameObject.activeInHierarchy);
        }
    }
}
