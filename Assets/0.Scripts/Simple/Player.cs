using UnityEngine;

public class Player : MonoBehaviour
{
    private IState currentState;

    private float speed = 5f;

    private int atkDamage = 10;

    void Start()
    {
        ChangeState(new PlayerMoveState(this, speed));
    }

    void Update()
    {
        currentState?.Tick();

        //위 아래는 동일하다. 그러나 위 방식은 에러 검출이 되지않음.
        //if(currentState != null)
        //{
        //    currentState.Tick();
        //}
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    void TakeDamage(int damage)
    {
        ChangeState(new PlayerHitState(this, currentState, atkDamage));
    }
}
