using UnityEngine;

public class GoldPickupEffect : PickupEffect
{
    [Header("골드 정보")]
    [SerializeField]
    [Min(1)]
    [Tooltip("외부에서 값이 지정되지 않았을 때 사용할 기본 골드 가치")]
    private int amount = 1;

    public int Amount => amount;

    // 이 코인의 골드 가치를 전달받은 값으로 교체한다.
    public void SetAmount(int value)
    {
        amount = Mathf.Max(1, value);
    }

    public override bool Apply(PlayerModel playerModel)
    {
        if (playerModel == null)
            return false;

        playerModel.AddGold(amount);

        return true;
    }
}