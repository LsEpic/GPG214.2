using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DashUnlock : MonoBehaviour
{
    public PlayerDash dashScript;
    public Canvas popup;
    private bool dashEnabled = false;

    void Start()
    {
        // Ensure the dash script is initially disabled
        if (dashScript != null)
        {
            dashScript.enabled = false;
        }

        popup.enabled = false;
    }

    public void EnableDash()
    {
        dashEnabled = true;
        ShowAchievement();

        if (dashScript != null)
        {
            dashScript.enabled = true;
        }

        Debug.Log("Dash Unlocked!");
    }

    public void ShowAchievement()
    {
        StartCoroutine(UnlockAchievementPopup());
    }

    public IEnumerator UnlockAchievementPopup()
    {
        popup.enabled = true;

        yield return new WaitForSeconds(4);

        popup.enabled = false;
    }

    public bool IsDashEnabled()
    {
        return dashEnabled;
    }
}
