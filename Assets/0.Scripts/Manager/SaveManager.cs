using System.IO;
using UnityEngine;

public class GameSaveData
{
    // 저장 해야 할 데이터

    // 임시 데이터
    public string playerName;
    public int level;
    public int gold;
    public int exp;
}

public class SaveManager : Singleton<SaveManager>
{
    public string savePath;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    [ContextMenu("Save")]
    public void Save()
    {
        GameSaveData data = new();
        data.playerName = "hero";

        if (data == null)
        {
            Debug.Log("data is not found");
            return;
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);

        Debug.Log($"Save Complete : {savePath}");
    }

    [ContextMenu("Load")]
    public GameSaveData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Save File is not found");
            return null;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log(data.playerName);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }

    }

    [ContextMenu("Delete")]
    public void Delete()
    {
        if (!File.Exists(savePath))
        {
            return;
        }

        File.Delete(savePath);
        Debug.Log("Save File Delete Complete");
    }
}
