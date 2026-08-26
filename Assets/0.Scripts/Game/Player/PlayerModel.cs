using UnityEngine;

public class PlayerModel : MonoBehaviour
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
}
