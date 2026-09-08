public class MonsterDeadState : IState
{
    private readonly Monster monster;

    public MonsterDeadState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.PrepareForDeath();

        monster.View.PlayDeath();
        monster.View.HideHPBar();

        monster.Death();
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}