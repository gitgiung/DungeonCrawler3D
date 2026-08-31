using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [Header("HP")]
    [SerializeField] private int currentHP;
    public int CurrentHP => currentHP;

    public bool IsDead => currentHP <= 0;

    public void Initialize(int maxHP)
    {
        currentHP = maxHP;
    }

    public void ReduceHP(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
    }
}
