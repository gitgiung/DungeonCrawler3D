using UnityEngine;

public class MonsterIdleState : IState
{
    private readonly Monster monster;

    public MonsterIdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.StopMoving();
        monster.Model.Target = null;
        monster.View.PlayIdle();
    }

    public void Tick()
    {
        Collider[] colliders = Physics.OverlapSphere(
            monster.transform.position,
            monster.Data.DetectRange,
            monster.Data.TargetLayer
        );

        if (colliders.Length == 0)
            return;

        monster.Model.Target = colliders[0].transform;
        monster.ChangeState(monster.ChaseState);
    }

    public void Exit()
    {
    }
}