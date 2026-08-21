using Unity.Hierarchy;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("Player Speed")]
    [SerializeField] private float walkSpeed = 5f;
    public float WalkSpeed
    {
        get { return walkSpeed; }
        set { walkSpeed = value; }
    }

    [SerializeField] private float sprintSpeed = 10f;
    public float SprintSpeed
    {
        get { return sprintSpeed; }
        set { sprintSpeed = value; }
    }

    [Header("Player Jump")]
    [SerializeField] private float jumpForce = 5f;
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

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
    [SerializeField] private float dashSpeed = 15f;
    public float DashSpeed
    {
        get { return dashSpeed; }
        set { dashSpeed = value; }
    }

    [SerializeField] private float dashDuration = 0.2f;
    public float DashDuration
    {
        get { return dashDuration; }
        set { dashDuration = value; }
    }
    [SerializeField] private GameObject dashShadow;
    public GameObject DashShadow
    {
        get { return dashShadow; }
    }

    [Header("Player HP")]
    [SerializeField] private int maxHP = 100;
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    [Header("Player Attack")]
    [SerializeField] private int attackDamage = 10;
    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
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
        set { attackHeight = value; }
    }

    [SerializeField, Range(0f, 3f)] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    [SerializeField, Range(1f, 5f)] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
        set { attackRadius = value; }
    }
}
