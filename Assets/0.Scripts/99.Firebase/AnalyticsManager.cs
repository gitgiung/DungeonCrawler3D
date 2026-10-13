using Firebase.Analytics;
using UnityEngine;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private void Start()
    {
        LogGameStart();
    }

    public void LogGameStart()
    {
        Debug.Log("[Analytics] Game Start");
        FirebaseAnalytics.LogEvent("game_start");
    }

    public void LogStageStart(int stageId)
    {
        FirebaseAnalytics.LogEvent("stage_start", new Parameter("stage_id", stageId));
    }

    public void LogStageClear(int stageId, int clearTime, int playerLevel)
    {
        FirebaseAnalytics.LogEvent("stage_clear",
            new Parameter("stage_id", stageId),
            new Parameter("clear_time", clearTime),
            new Parameter("player_level", playerLevel)
            );
    }

    public void CurrencyEarn(string type, int amount, string source)
    {
        FirebaseAnalytics.LogEvent("currency_earn",
            new Parameter("currency_type", type),
            new Parameter("amount", amount),
            new Parameter("source", source)
            );
    }

    public void Log(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }
}
