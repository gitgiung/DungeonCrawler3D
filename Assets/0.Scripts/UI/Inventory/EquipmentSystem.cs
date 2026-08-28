using UnityEngine;

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    public EquipmentSlot[] slots;
    public EquipmentSlot SelectSlot { get; set; }

    public int EquipTotalDamage()
    {
        int totalValue = 0;

        foreach (EquipmentSlot slot in slots)
        {
            if (slot.Data != null)
            {
                totalValue += slot.Data.Damage;
            }
        }
        return totalValue;
    }

    public int EquipTotalDefence()
    {
        int totalValue = 0;

        foreach (EquipmentSlot slot in slots)
        {
            if (slot.Data != null)
            {
                totalValue += slot.Data.Defence;
            }
        }
        return totalValue;
    }

    public float EquipTotalSpeed()
    {
        float totalValue = 0;

        foreach(EquipmentSlot slot in slots)
        {
            if (slot.Data != null)
            {
                totalValue += slot.Data.Speed;
            }
        }
        return totalValue;
    }
}
