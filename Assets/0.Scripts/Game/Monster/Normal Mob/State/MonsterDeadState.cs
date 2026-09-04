using UnityEngine;

public class MonsterDeadState : IState
{
    private Monster monster;
    public MonsterDeadState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.View.PlayDeath();

        monster.Death();
    }

    public void Exit()
    {

    }

    public void Tick()
    {

    }
}
