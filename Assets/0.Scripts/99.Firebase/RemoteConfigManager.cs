using UnityEngine;
using Firebase.RemoteConfig;
using System.Collections.Generic;
using Firebase.Extensions;

public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    private FirebaseRemoteConfig remoteConfig;

    private void Start()
    {
        remoteConfig = FirebaseRemoteConfig.DefaultInstance;
    }

    private void SetDefaults()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            {"enemy_damage", 10},
            {"stage_reward", 1000},
            {"event_bonus", 2.0},
            {"maintenance", false},
            {"minimum_version", "1.0.0"}
        };

        remoteConfig
            .SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task => 
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("[Remote Config] Defaults Fail");
                    return;
                }

                Debug.Log("[Remote Config] Default Ready");
            });
    }

    private void FetchRemoteConfig()
    {
        Debug.Log("[Remote Config] Fetch Start");

        remoteConfig
            .FetchAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("[Remote Config] Fetch Fail");
                    return;
                }

                remoteConfig
                .ActivateAsync()
                .ContinueWithOnMainThread(activateTask =>
                {
                    if (activateTask.IsFaulted)
                    {
                        Debug.LogError("[Remote Config] Activate Fail");
                        return;
                    }

                    Debug.Log("[Remote Config] Activate Success");
                    ApplyValues();
                });

                Debug.Log("[Remote Config] Fetch Success");
            });
    }

    private void ApplyValues()
    {
        long enemyDamage = remoteConfig.GetValue("enemy_damage").LongValue;
        long stageReward = remoteConfig.GetValue("stage_reward").LongValue;
        double eventBonus = remoteConfig.GetValue("event_bonus").DoubleValue;
        string maintenance = remoteConfig.GetValue("maintenance").StringValue;
        string minimumVersion = remoteConfig.GetValue("minimum_version").StringValue;

        Debug.Log($"Minimum Version: {minimumVersion}");
    }


}
