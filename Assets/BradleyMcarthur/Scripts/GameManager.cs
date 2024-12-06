using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using System.Text;

public class GameManager : MonoBehaviour
{
    #region Classes
    private SaveData saveData;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private PlayerDash dashScript;
    #endregion

    public Canvas popup;

    public string folderPath = Application.streamingAssetsPath;
    public string fileName = "Save Data";
    private string destinationPath;

    // Start is called before the first frame update
    void Start()
    {
        saveData = SaveManager.Instance.saveData;

        if (folderPath != null) //check that streaming assets folder exists
        {
            destinationPath = Path.Combine(folderPath, fileName);
        }
        else
        {
            Debug.Log("No Streaming Assets Folder");
        }

        // Ensure the dash script is initially disabled
        if (dashScript != null)
        {
            dashScript.enabled = false;
        }

        popup.enabled = false;
        saveData.OnDashUnlocked += EnableDash; //subscribes to event
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position vector
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindWithTag("Player"); // Find player in the scene
            if (player != null)
            {
                saveData.SetLastCheckpoint(player.transform.position);
            }

            Save();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Load();
        }

        //this allows player to dash when loading save data if theyve already unlocked it.
        if(saveData.dashUnlocked == true)
        {
            dashScript.enabled = true;
        }
    }

    //unlocks dash script
    public void EnableDash()
    {
        ShowAchievement();

        if (dashScript != null)
        {
            dashScript.enabled = true;
        }
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

    void Save()
    {
        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(SaveData));
        string xml = "";

        using (StringWriter writer = new StringWriter())
        {
            xmlSerialiser.Serialize(writer, saveData);
            xml = writer.ToString();
        }

        Debug.Log(xml);

        //convert the string into bytes
        byte[] xmlByteArray = Encoding.UTF8.GetBytes(xml);

        string convertedString = System.Convert.ToBase64String(xmlByteArray);

        byte[] base64ByteArray = Encoding.UTF8.GetBytes(convertedString);

        Debug.Log(base64ByteArray.Length);

        FileStream stream = new FileStream(destinationPath,FileMode.Create, FileAccess.Write);

        stream.Write(base64ByteArray, 0, base64ByteArray.Length);

        stream.Close();

        Debug.Log("Data Saved!");
    }

    void Load()
    {
        if(File.Exists(destinationPath))
        {
            byte[] base64ByteArray;

            using(FileStream fileStream = new FileStream(destinationPath,FileMode.Open, FileAccess.Read))
            {
                base64ByteArray = new byte[fileStream.Length];
                fileStream.Read(base64ByteArray, 0, base64ByteArray.Length);
            }

            string base64String = Encoding.UTF8.GetString(base64ByteArray);

            byte[] xmlByteArray = System.Convert.FromBase64String(base64String);

            string xmlString = Encoding.UTF8.GetString(xmlByteArray);

            Debug.Log(xmlString);

            //Converts the 64 back to String

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveData));

            using(StringReader reader = new StringReader(xmlString))
            {
                SaveManager.Instance.saveData = (SaveData)xmlSerializer.Deserialize(reader);
                saveData = SaveManager.Instance.saveData;
            }

            Debug.Log("Load Complete");

            if (saveData.lastCheckpointPosition != Vector3.zero)
            {
                
                GameObject player = GameObject.FindWithTag("Player"); // Find player in the scene
                if (player != null)
                {
                    player.transform.position = saveData.lastCheckpointPosition;
                }
            }
        }
        else
        {
            Debug.Log("Couldnt load data!");
        }
    }
}
