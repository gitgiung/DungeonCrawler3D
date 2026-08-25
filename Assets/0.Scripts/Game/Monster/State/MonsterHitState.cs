using UnityEngine;

public class MonsterHitState : IState
{
    private Monster monster;
    public MonsterHitState(Monster monster)
    {
        this.monster = monster;
    }

    private float stunTimer = 0f;
    private float stunTime = 2f;

    public void Enter()
    {
        Debug.Log("몬스터 피격");
        monster.View.PlayHit();
    }

    public void Exit()
    {
        Debug.Log("몬스터 재추적");
    }

    public void Tick()
    {
        stunTimer += Time.deltaTime;
        if(stunTimer > stunTime)
        {
            monster.ChangeState(new MonsterIdleState(monster));
        }
    }
}
