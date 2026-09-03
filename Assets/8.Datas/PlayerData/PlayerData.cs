using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player HP")]
    [SerializeField] private int maxHP;
    public int MaxHP
    {
        get { return maxHP; }
    }

    [Header("Player Speed")]
    [SerializeField, Min(0f)] private float walkSpeed;
    public float WalkSpeed
    {
        get { return walkSpeed; }
    }

    [SerializeField, Min(0f)] private float sprintSpeed;
    public float SprintSpeed
    {
        get { return sprintSpeed; }
    }

    [SerializeField, Min(0f)] private float rotationSpeed = 720f;
    public float RotationSpeed
    {
        get { return rotationSpeed; }
    }

    [Header("Player Jump")]
    [SerializeField, Min(0f)] private float jumpHeight = 1.3f;
    public float JumpHeight
    {
        get { return jumpHeight; }
    }

    [SerializeField, Range(-50f, -0.1f)] private float gravity = -9.81f;
    public float Gravity
    {
        get { return gravity; }
    }

    [Header("Player Dash")]
    [SerializeField, Min(0f)] private float dashSpeed;
    public float DashSpeed
    {
        get { return dashSpeed; }
    }

    [SerializeField, Min(0f)] private float dashDuration;
    public float DashDuration
    {
        get { return dashDuration; }
    }

    [Header("Player Attack")]
    [SerializeField] private int attackDamage;
    public int AttackDamage
    {
        get { return attackDamage; }
    }

    [SerializeField] private LayerMask targetLayer;
    public LayerMask TargetLayer
    {
        get { return targetLayer; }
    }

    [SerializeField, Range(0f, 3f)] private float attackHeight;
    public float AttackHeight
    {
        get { return attackHeight; }
    }

    [SerializeField, Range(0f, 3f)] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
    }

    [SerializeField, Range(1f, 5f)] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
    }
}
