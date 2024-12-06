using Gamekit3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 lastCheckpointPosition;
    public int deathCount = 0; 
    public bool dashUnlocked = false;

    public event Action OnDashUnlocked; //event for when dash bool is set to true

    public void AddDeath()
    {
        deathCount++;
        Debug.Log("Death Added, total Deaths: " + deathCount);
    }

    public bool DashUnlocked
    {
        get => dashUnlocked;
        set
        {
            if (!dashUnlocked && value) //trigger if changing from false to true
            {
                dashUnlocked = value;
                OnDashUnlocked?.Invoke(); //start event
            }
            else
            {
                dashUnlocked = value;
            }
        }
    }
    public void SetLastCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        Debug.Log("Last checkpoint set to: " + position);
    }
}
