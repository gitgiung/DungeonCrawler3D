using UnityEngine;
using System;

public class PlayerModel : MonoBehaviour
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnHPChanged;
    public event Action<int> OnExpChanged;
    public event Action<int> OnLevelChanged;
    public event Action OnStatsChanged;
    public event Action<bool> OnDeathStateChanged;

    private PlayerData data;
    private int equipmentMaxHP;
    private int equipmentDamage;
    private int equipmentDefence;
    private float equipmentSpeed;

    public int MaxHP { get; private set; }
    public int AttackDamage { get; private set; }
    public int Defence { get; private set; }
    public float WalkSpeed { get; private set; }
    public float SprintSpeed { get; private set; }
    
    public bool IsDead { get; private set; }
    public int CurrentHP { get; private set; }
    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int Level { get; private set; } = 1;
    public int RequiredExp => data == null ? 0 : data.GetRequiredExp(Level);
    
    public void Init(PlayerData playerData)
    {
        data = playerData;
        Level = 1;
        Gold = 0;
        Exp = 0;
        equipmentDefence = 0;
        equipmentMaxHP = 0;
        equipmentDamage = 0;
        equipmentSpeed = 0f;

        CalculateStats(false, false);
        SetHP(MaxHP);
    }

    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public void AddExp(int amount)
    {
        if (amount <= 0 || data == null)
            return;

        if (Level >= data.MaxLevel)
        {
            Exp = 0;
            OnExpChanged?.Invoke(Exp);
            return;
        }

        int previousLevel = Level;
        int previousExp = Exp;
        int availableExp = Exp + amount;

        while (Level < data.MaxLevel)
        {
            int requiredExp = data.GetRequiredExp(Level);

            if (requiredExp <= 0 || availableExp < requiredExp)
                break;

            availableExp -= requiredExp;
            Level++;
        }

        Exp = Level >= data.MaxLevel
            ? 0
            : availableExp;

        if (Level != previousLevel)
        {
            CalculateStats(true);
            OnLevelChanged?.Invoke(Level);
        }

        if (Exp != previousExp)
            OnExpChanged?.Invoke(Exp);
    }

    private void SetHP(int value)
    {
        int setHP = Mathf.Clamp(value, 0, MaxHP);
        bool wasDead = IsDead;
        bool hpChanged = CurrentHP != setHP;

        CurrentHP = setHP;
        IsDead = CurrentHP <= 0;

        if (hpChanged)
            OnHPChanged?.Invoke(CurrentHP);

        if (wasDead != IsDead)
            OnDeathStateChanged?.Invoke(IsDead);
    }

    public bool ReduceHP(int damage)
    {
        if (damage <= 0 || IsDead)
            return false;

        SetHP(CurrentHP - damage);
        return true;
    }

    public void LoadData(int level, int gold, int exp, int currentHP)
    {
        if (data == null)
            return;
        
        Level = Mathf.Max(1, level);
        Gold = Mathf.Max(0, gold);
        Exp = Mathf.Max(0, exp);

        SetHP(currentHP);

        OnGoldChanged?.Invoke(Gold);
        OnExpChanged?.Invoke(Exp);
        OnLevelChanged?.Invoke(Level);
    }

    public void SetEquipmentStats(int maxhp, int damage, int defence, float speed)
    {
        equipmentMaxHP = Mathf.Max(0, maxhp);
        equipmentDamage = Mathf.Max(0, damage);
        equipmentDefence = Mathf.Max(0, defence);
        equipmentSpeed = Mathf.Max(0f, speed);

        if (data == null)
            return;

        CalculateStats(false);
    }

    private bool CalculateStats(bool healMaxHpIncrease, bool notify = true)
    {
        return false;
    }
}
