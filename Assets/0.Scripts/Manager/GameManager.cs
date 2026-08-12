using UnityEngine;

public enum GameState
{
    Playing,
    Pause,
    Stop
}

public class GameManager : Singleton<GameManager>
{
    public int Score { get; set; }
    public GameState State { get; set; }

    private void Awake()
    {
        State = GameState.Playing;

        //Scene이 변경되도 사라지지 않는다
        DontDestroyOnLoad(gameObject);
    }
}
