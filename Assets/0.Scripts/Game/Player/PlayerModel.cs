using UnityEngine;
using System;

public class PlayerModel : MonoBehaviour
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnHPChanged;
    public event Action<int> OnExpChanged;

    [Header("Player Dash")]
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow
    {
        get { return dashShadow; }
    }

    public int CurrentHP { get; private set; } = 200;
    public bool IsDead { get; private set; }
    public void ReduceHP(int damage)
    {
        CurrentHP = Mathf.Max(CurrentHP - damage, 0);

        OnHPChanged?.Invoke(CurrentHP);

        if(CurrentHP <= 0)
        {
            IsDead = true;
        }
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
        Debug.Log($"현재 경험치: {Exp}");
        OnExpChanged?.Invoke(Exp);
    }

    public void LoadData(int level, int gold, int exp, int currentHP)
    {
        Level = level;
        Gold = gold;
        Exp = exp;
        CurrentHP = currentHP;
    }
}
