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
        Debug.Log("∏ÛΩ∫≈Õ ªÁ∏¡");
    }

    public void Exit()
    {

    }

    public void Tick()
    {

    }
}
