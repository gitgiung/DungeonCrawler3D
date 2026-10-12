using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    private FirebaseDatabase database;

    public TestTempData CurrentTestData {  get; private set; }

    private void Start()
    {
        InitializeDatabase();
    }

    // Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadOrCreatePlayer();
        }
    }

    private void InitializeDatabase()
    {
        database = FirebaseDatabase.DefaultInstance;

        Debug.Log("[Database] Initialize");
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(CurrentTestData);

        DatabaseReference reference =
            database
            .GetReference("user")
            .Child(CurrentTestData.uid);

        reference.SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("[Database] Player Data Save Failed");
                    Debug.LogException(task.Exception);
                    return;
                }

                Debug.Log("[Database] Player Data Save Success");
            });
    }

    public void LoadPlayerData()
    {
        string uid = AuthManager.Instance.CurrentUser.UserId;

        DatabaseReference reference =
            database
            .GetReference("user")
            .Child(uid);

        database
            .GetReference("user")
            .Child(uid)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("[Database] Player Data Load Failed");
                    Debug.LogException(task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                if (!snapshot.Exists)
                {
                    Debug.Log("[Database] Player Data Not Found");
                    return;
                }

                string json = snapshot.GetRawJsonValue();
                TestTempData data = JsonUtility.FromJson<TestTempData>(json);

                Debug.Log($"[Database] Player Data Load Success: {data.gold}");
            });

        //reference
        //    .GetValueAsync()
        //    .ContinueWithOnMainThread(task =>
        //    {
        //        if (task.IsFaulted)
        //        {
        //            Debug.Log("[Database] Player Data Load Failed");
        //            Debug.LogException(task.Exception);
        //            return;
        //        }

        //        DataSnapshot snapshot = task.Result;
        //        Debug.Log($"[Database] Snapshot: {snapshot.GetRawJsonValue()}");
        //        Debug.Log("[Database] Player Data Load Success");
        //    });
    }

    private void LoadOrCreatePlayer()
    {
        string uid = AuthManager.Instance.CurrentUser.UserId;

        DatabaseReference reference =
            database
            .GetReference("user")
            .Child(uid);

        reference
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("[Database] Player Data Load Failed");
                    Debug.LogException(task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    LoadExistionPlayer(snapshot);
                }
                else
                {
                    CreateNewPlayer(uid);
                }
            });
    }

    private void LoadExistionPlayer(DataSnapshot snapshot)
    {
        string json = snapshot.GetRawJsonValue() ;
        CurrentTestData = JsonUtility.FromJson<TestTempData>(json);
        Debug.Log("[Database] Already Exist");
    }

    private void CreateNewPlayer(string uid)
    {
        CurrentTestData = new(uid);
        SavePlayerData();
        Debug.Log("[Database] Create New Player Data");
    }
}
