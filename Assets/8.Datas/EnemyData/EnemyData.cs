using UnityEngine;

public enum MonsterType
{
    Normal,
    Boss
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "MonsterData/Data")]
public class EnemyData : ScriptableObject
{
    [Header("ID / Type")]
    [SerializeField] private int monsterID;
    public int MonsterID => monsterID;

    [SerializeField] private MonsterType monsterType;
    public MonsterType MonsterType => monsterType;

    [Header("Targeting Player")]
    [SerializeField] private LayerMask targetLayer;
    public LayerMask TargetLayer { get { return targetLayer; } }

    [SerializeField] private float detectRange;
    public float DetectRange { get { return detectRange; } }

    [SerializeField] private float loseTargetRange;
    public float LoseTargetRange { get { return loseTargetRange; } }

    [Header("Combat")]
    [SerializeField] private int attackDamage;
    public int AttackDamage { get { return attackDamage; } }

    [SerializeField] private float attackDelay;
    public float AttackDelay { get { return attackDelay; } }

    [SerializeField] private float attackRange;
    public float AttackRange { get { return attackRange; } }

    [SerializeField] private float stunTime;
    public float StunTime { get { return stunTime; } }

    [Header("HP")]
    [SerializeField] private int maxHP;
    public int MaxHP { get { return maxHP; } }

    [Header("Movement")]
    [SerializeField] private int moveSpeed;
    public int MoveSpeed { get { return moveSpeed; } }

    
}
