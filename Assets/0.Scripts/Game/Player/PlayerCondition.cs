using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHP = 100;

    public int CurrentHP { get; private set; }
    public int MaxHP => maxHP;

    private void Start()
    {
        CurrentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;

        Debug.Log($"입은 피해: {damage}, 플레이어 체력: {CurrentHP}");

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is died");
    }
}
