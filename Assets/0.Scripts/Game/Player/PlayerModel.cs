using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public event System.Action OnChanged;

    [Header("Player Dash")]
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow
    {
        get { return dashShadow; }
    }

    [Header("Player Info")]
    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get { return currentHP; }
        private set { currentHP = value; }
    }

    public bool IsDead { get; private set; }

    public void ReduceHP(int damage)
    {
        CurrentHP = Mathf.Max(CurrentHP - damage, 0);

        if(CurrentHP <= 0)
        {
            IsDead = true;
        }
    }

    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int Level { get; private set; }

    public void AddGold(int amount)
    {
        if (amount < 0)
            return;

        Gold += amount;
        Debug.Log($"골드: {Gold}");
        OnChanged?.Invoke();
    }

    public void AddExp(int amount)
    {
        if (amount < 0)
            return;

        Exp += amount;
        Debug.Log($"경험치: {Exp}");
        OnChanged?.Invoke();
    }
}
