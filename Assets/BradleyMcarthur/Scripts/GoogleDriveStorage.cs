using UnityEngine;
using System.Text.RegularExpressions;

public static class GoogleDriveStorage
{
    public static string ConvertToDirectDownloadLink(string rawLink)
    {
        //https://drive.google.com/file/d/1W5dGokb2FSIj30UQWBkNUHc5vfEyAWi4/view?usp=drive_link/
        
        Debug.Log("Raw Link: " + rawLink);

        //regular expression to extract file id from google drive format
        Match match = Regex.Match(rawLink, @"drive\.google\.com\/file\/d\/([a-zA-Z0-9_-]+)"); 

        string result = string.Empty;

        if(match.Success)
        {
            string id = match.Groups[1].Value;
            result = "https://drive.google.com/uc?export=download&id=" + id;
            Debug.Log(result);
        }
        else
        {
            Debug.LogError("Invalid Drive Link");
        }

        return result;
    }
}
