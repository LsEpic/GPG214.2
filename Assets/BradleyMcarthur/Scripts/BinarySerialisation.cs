using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using System.Text;

public class BinarySerialisation : MonoBehaviour
{
    [SerializeField] private SaveData progress;

    public string folderPath = Application.streamingAssetsPath;
    public string fileName = "Save Data";
    private string destinationPath;

    // Start is called before the first frame update
    void Start()
    {
        destinationPath = Path.Combine(folderPath, fileName);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Load();
        }
    }

    void Save()
    {
        XmlSerializer xmlSerialiser = new XmlSerializer(typeof(SaveData));
        string xml = "";

        using (StringWriter writer = new StringWriter())
        {
            xmlSerialiser.Serialize(writer, progress);
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
                progress = (SaveData)xmlSerializer.Deserialize(reader);
            }

            Debug.Log("Load Complete");
        }
        else
        {
            Debug.Log("Couldnt load data!");
        }
    }
}
