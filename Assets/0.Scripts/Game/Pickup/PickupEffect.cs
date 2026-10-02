using UnityEngine;

public abstract class PickupEffect : MonoBehaviour
{
    /// 플레이어에게 픽업 효과를 적용한다.
    /// 성공적으로 적용되면 true를 반환한다.
    public abstract bool Apply(PlayerModel playerModel);
}