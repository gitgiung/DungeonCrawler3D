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
        Debug.Log("몬스터 대기상태 진입");
    }

    public void Exit()
    {
        Debug.Log("몬스터 대기상태 해제");
    }

    public void Tick()
    {
        //몬스터 플레이어 감지
        Collider[] cols = Physics.OverlapSphere(
            monster.transform.position, monster.DetectRange, monster.TargetLayer);

        if (cols.Length == 0)
            return;

        monster.Target = cols[0].transform;
        monster.ChangeState(new MonsterChaseState(monster));
    }
}
