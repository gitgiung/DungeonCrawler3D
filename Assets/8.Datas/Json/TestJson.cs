using UnityEngine;

public class TestJson : MonoBehaviour
{
    void Start()
    {
        TestSaveData data = new()
        {
            playerName = "Huscarl",
            level = 2,
            gold = 100,
            exp = 100
        };

        // 저장 & 보안코드
        string jsonDataString = JsonUtility.ToJson(data, true);
        Debug.Log(jsonDataString);

        // 불러오기
        TestSaveData loadData = JsonUtility.FromJson<TestSaveData>(jsonDataString);
        Debug.Log(loadData.playerName);

        Debug.Log(Application.persistentDataPath);
    }

}
