using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    private PlayerController player;

    public bool HasDead { get; private set; }

    public int CurrentHP { get; private set; }
    public int MaxHP => model.MaxHP;

    private PlayerModel model;

    public void Initialize(PlayerModel model)
    {
        this.model = model;
    }

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        CurrentHP = model.MaxHP;
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
        HasDead = true;
        player.ChangeState(player.DeadState);
    }

    public void Revive()
    {
        HasDead = false;
    }
}
