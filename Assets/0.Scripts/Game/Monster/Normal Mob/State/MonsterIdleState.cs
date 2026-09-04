using UnityEngine;

public class MonsterIdleState : IState
{
    private Monster monster;
    public MonsterIdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.View.PlayIdle();
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        //몬스터 플레이어 감지
        Collider[] cols = Physics.OverlapSphere(
            monster.transform.position,
            monster.Data.DetectRange,
            monster.Data.TargetLayer);

        if (cols.Length == 0)
            return;

        monster.Model.Target = cols[0].transform;
        monster.ChangeState(new MonsterChaseState(monster));
    }
}
