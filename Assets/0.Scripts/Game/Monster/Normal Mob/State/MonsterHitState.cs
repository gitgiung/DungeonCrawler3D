using UnityEngine;

public class MonsterHitState : IState
{
    private Monster monster;
    public MonsterHitState(Monster monster)
    {
        this.monster = monster;
    }

    private float stunTimer = 0f;

    public void Enter()
    {
        monster.View.PlayHit();
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        stunTimer += Time.deltaTime;
        if(stunTimer > monster.Data.StunTime)
        {
            monster.ChangeState(new MonsterIdleState(monster));
        }
    }
}
