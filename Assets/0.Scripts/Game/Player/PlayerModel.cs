using UnityEngine;
using System;

public class PlayerModel : MonoBehaviour
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnHPChanged;
    public event Action<int> OnExpChanged;
    public event Action<bool> OnDeathStateChanged;

    [Header("Player Dash")]
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow
    {
        get { return dashShadow; }
    }

    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public bool IsDead { get; private set; }

    public void Init(int maxHP)
    {
        MaxHP = Mathf.Max(1, maxHP);
        SetHP(MaxHP);
    }

    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int MaxExp { get; private set; } = 500;
    public int Level { get; private set; } = 1;

    public void AddGold(int amount)
    {
        if (amount < 0)
            return;

        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public void AddExp(int amount)
    {
        if (amount < 0)
            return;

        Exp += amount;
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
        Level = Mathf.Max(1, level);
        Gold = Mathf.Max(0, gold);
        Exp = Mathf.Max(0, exp);

        SetHP(currentHP);

        OnGoldChanged?.Invoke(Gold);
        OnExpChanged?.Invoke(Exp);
    }
}
