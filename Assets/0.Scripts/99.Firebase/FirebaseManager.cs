using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseApp app;

    public bool IsReady { get; private set; }

    private void Start()
    {
        InitFirebase();
    }

    private void InitFirebase()
    {
        Debug.Log("[Firebase] Initialize");

        FirebaseApp
            .CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("[Firebase] Task Failed");
                    return;
                }

                DependencyStatus status = task.Result;

                if (status != DependencyStatus.Available)
                {
                    IsReady = false;
                    Debug.LogError("[Firebase] Dependency Error: " + status);
                    return;
                }

                app = FirebaseApp.DefaultInstance;

                IsReady = true;

                Debug.Log("[Firebase] Ready");

                AuthManager.Instance.Init();
            });
    }
}