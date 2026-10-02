using UnityEngine;

public enum PlayerStatType
{
    AttackPower,
    MoveSpeed,
    MaxHP
}

public class StatPickupEffect : PickupEffect
{
    [Header("스탯 증가 정보")]
    [SerializeField]
    [Tooltip("증가시킬 플레이어 스탯")]
    private PlayerStatType statType;

    [SerializeField]
    [Min(0.01f)]
    [Tooltip("증가시킬 수치")]
    private float amount = 1f;

    public PlayerStatType StatType => statType;
    public float Amount => amount;

    public override bool Apply(PlayerModel playerModel)
    {
        if (playerModel == null)
            return false;

        if (amount <= 0f)
            return false;

        playerModel.AddPickupStat(statType, amount);

        return true;
    }
}