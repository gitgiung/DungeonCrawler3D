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
    [SerializeField] private float walkSpeed;
    public float WalkSpeed
    {
        get { return walkSpeed; }
    }

    [SerializeField] private float sprintSpeed;
    public float SprintSpeed
    {
        get { return sprintSpeed; }
    }

    [Header("Player Jump")]
    [SerializeField] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
    }

    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    public float DashSpeed
    {
        get { return dashSpeed; }
    }

    [SerializeField] private float dashDuration;
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
