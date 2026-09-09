using UnityEngine;

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private EquipmentSlot[] slots;

    public EquipmentSlot SelectSlot { get; set; }

    private void Start()
    {
        RefreshStats();
    }

    public void RefreshStats()
    {
        if (playerModel == null)
        {
            Debug.LogError(
                "PlayerModel is not assigned to EquipmentSystem.",
                this
            );
            return;
        }

        int totalMaxHP = 0;
        int totalDamage = 0;
        int totalDefence = 0;
        float totalSpeed = 0f;

        if (slots != null)
        {
            foreach (EquipmentSlot slot in slots)
            {
                if (slot == null || slot.Data == null)
                    continue;

                totalMaxHP += slot.Data.MaxHP;
                totalDamage += slot.Data.Damage;
                totalDefence += slot.Data.Defence;
                totalSpeed += slot.Data.Speed;
            }
        }

        playerModel.SetEquipmentBonuses(
            totalMaxHP,
            totalDamage,
            totalDefence,
            totalSpeed
        );
    }
}
