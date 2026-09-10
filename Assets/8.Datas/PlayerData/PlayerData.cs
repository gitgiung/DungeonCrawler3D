using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player HP")]
    [SerializeField] private int maxHP = 100;
    public int MaxHP => maxHP;

    [Header("Player Speed")]
    [SerializeField, Min(0f)] private float walkSpeed = 3;
    public float WalkSpeed => walkSpeed;

    [SerializeField, Min(0f)] private float sprintSpeed = 6;
    public float SprintSpeed => sprintSpeed;

    [SerializeField, Min(0f)] private float rotationSpeed = 720f;
    public float RotationSpeed => rotationSpeed;

    [Header("Player Jump")]
    [SerializeField, Min(0f)] private float jumpHeight = 1.3f;
    public float JumpHeight => jumpHeight;

    [SerializeField, Range(-50f, -0.1f)] private float gravity = -9.81f;
    public float Gravity => gravity;

    [Header("Player Dash")]
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow => dashShadow;

    [SerializeField, Min(0f)] private float dashSpeed = 15;
    public float DashSpeed => dashSpeed;

    [SerializeField, Min(0f)] private float dashDuration = 0.2f;
    public float DashDuration => dashDuration;

    [Header("Player Attack")]
    [SerializeField] private int attackDamage = 5;
    public int AttackDamage => attackDamage;

    [SerializeField] private LayerMask targetLayer;
    public LayerMask TargetLayer => targetLayer;

    [SerializeField, Range(0f, 3f)] private float attackHeight = 1f;
    public float AttackHeight => attackHeight;

    [SerializeField, Range(0f, 3f)] private float attackRange = 1.5f;
    public float AttackRange => attackRange;

    [SerializeField, Range(1f, 5f)] private float attackRadius = 1.5f;
    public float AttackRadius => attackRadius;

    [Header("Player Level")]
    [SerializeField, Min(0)] private int maxHpPerLevel = 20;
    public int MaxHpPerLevel => maxHpPerLevel;

    [SerializeField, Min(0)] private int attackDamagePerLevel = 2;
    public int AttackDamagePerLevel => attackDamagePerLevel;

    // 현재는 고정 수치
    [SerializeField]
    private int[] maxExpByLevel =
    {
        100,
        200,
        350,
        550
    };

    public int MaxLevel => 5;

    // 레벨 당 얻어야 하는 경험치량 계산
    public int GetMaxExp(int level)
    {
        if (level < 1 || level >= MaxLevel)
            return 1;

        int index = level - 1;

        if (maxExpByLevel == null || index >= maxExpByLevel.Length)
            return 1;

        return Mathf.Max(1, maxExpByLevel[index]);
    }
}
