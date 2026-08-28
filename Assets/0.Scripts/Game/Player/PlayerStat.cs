using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int baseDamage;
    [SerializeField] private int baseDefence;
    [SerializeField] private float baseSpeed;

    public int MaxHP { get; private set; }
    public int BaseDamage { get; private set; }
    public int BaseDefence { get; private set; }
    public float BaseSpeed { get; private set; }

    private void Awake()
    {
        ResetStat();
    }

    public void ResetStat()
    {
        MaxHP = maxHP;
        BaseDamage = baseDamage;
        BaseDefence = baseDefence;
        BaseSpeed = baseSpeed;
    }

    public int FinalDamage()
    {
        return BaseDamage
            + EquipmentSystem.Instance.EquipTotalDamage();
    }
}
