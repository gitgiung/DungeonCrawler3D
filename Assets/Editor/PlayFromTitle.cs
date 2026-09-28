using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PlayFromTitle
{
    private const string TitlePath = "Assets/9.Scenes/Title.unity";

    [MenuItem("Tools/Play Mode/Start From Title")]
    private static void Enable()
    {
        EditorSceneManager.playModeStartScene =
            AssetDatabase.LoadAssetAtPath<SceneAsset>(TitlePath);

        Debug.Log("Play Mode Ω√¿€ æ¿: Title");
    }

    [MenuItem("Tools/Play Mode/Start From Current Scene")]
    private static void Disable()
    {
        EditorSceneManager.playModeStartScene = null;

        Debug.Log("Play Mode Ω√¿€ æ¿: «ˆ¿Á æ¿");
    }
}