using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    public string[] sceneNames =
    {
        "Title",
        "Loading",
        "Lobby",
        "0.TestRoom" //Dungeon01 대체
    };

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void TitleLoadScene()
    {
        Loading.LoadScene(sceneNames[0]);
    }

    public void LobbyLoadScene()
    {
        Loading.LoadScene(sceneNames[2]);
    }

    public void Dungeon01LoadScene()
    {
        Loading.LoadScene(sceneNames[3]);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
