using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public GameAnalytics analyticsData = new GameAnalytics();
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        analyticsData.timePlayed = Time.time - startTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            analyticsData.jumpCount++;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowAnalytics();
        }
    }

    public void EnemyDefeated()
    {
        analyticsData.enemiesDefeated++;
    }

    public void ShowAnalytics()
    {
        Debug.Log($"Enemies Defeated: {analyticsData.enemiesDefeated}");
        Debug.Log($"Player has jumped: {analyticsData.jumpCount} times!");
        Debug.Log($"Time Played: {analyticsData.timePlayed:F2} seconds");
    }
}
