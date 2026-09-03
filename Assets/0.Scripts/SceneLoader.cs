using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    public string[] sceneNames =
    {
        "Title",
        "Loading",
        "Lobby",
        "0.TestRoom" //Dungeon01 ¥Î√º
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
}
