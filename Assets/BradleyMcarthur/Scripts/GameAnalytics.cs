using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAnalytics
{
    public float timePlayed;
    public int jumpCount;
    public int enemiesDefeated;

    public GameAnalytics()
    {
        timePlayed = 0;
        jumpCount = 0;
        enemiesDefeated = 0;
    }
}
