using UnityEngine;

public enum GameState
{
    Playing,
    Pause,
    Stop
}

public class GameManager : Singleton<GameManager>
{
    public GameState State { get; set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        State = GameState.Playing;

        //Scene이 변경되도 사라지지 않는다
        DontDestroyOnLoad(gameObject);
    }
}
