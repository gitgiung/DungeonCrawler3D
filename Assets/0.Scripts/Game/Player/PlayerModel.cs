using UnityEngine;

public class PlayerModel : MonoBehaviour, IDamageable
{
    [Header("Player Jump")]
    [SerializeField] private Transform groundCheck;
    public Transform GroundCheck
    {
        get { return groundCheck; }
    }

    [SerializeField] private float groundCheckRadius = 0.1f;
    public float GroundCheckRadius
    {
        get { return groundCheckRadius; }
        set { groundCheckRadius = value; }
    }

    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer
    {
        get { return groundLayer; }
    }

    [Header("Player Dash")]
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow
    {
        get { return dashShadow; }
    }

    [Header("Player HP")]
    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public bool IsDead { get; private set; }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);
        Debug.Log($"플레이어가 입은 피해: {damage}, 플레이어 체력: {CurrentHP}");

        if(CurrentHP <= 0)
        {
            IsDead = true;
        }
    }
}
