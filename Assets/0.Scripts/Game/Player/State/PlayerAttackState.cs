using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerController player;

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Attack Enter");
        
    }

    public void Exit()
    {
        Debug.Log("Attack Exit");
    }

    public void Tick()
    {
        //공격 애니메이션 끝났는지 확인 후 Idle 또는 Move로 전환 가능

        if (!player.Combat.HasAttack)
        {
            player.ChangeState(player.IdleState);
        }
    }
}
