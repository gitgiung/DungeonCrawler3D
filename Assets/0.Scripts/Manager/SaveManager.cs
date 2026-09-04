using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string savePath;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );
    }

    public void Save(GameSaveData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Save Data is null");
            return;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Save Complete : {savePath}");
    }

    public GameSaveData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Save File is not found");
            return null;
        }

        string json = File.ReadAllText(savePath);

        GameSaveData data =
            JsonUtility.FromJson<GameSaveData>(json);

        Debug.Log($"Load Complete : {data.player.playerName}");

        return data;
    }

    public void Delete()
    {
        if (!File.Exists(savePath))
            return;

        File.Delete(savePath);

        Debug.Log("Save File Delete Complete");
    }
}