using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FileMetaData", menuName = "Metadata/File Metadata")]
public class MetadataScriptableObject : ScriptableObject
{
    [Header("Meta Data")]
    public string fileName = "file_metadata"; //default file name
    public string extension = ".json";
    public string metadataFileLink; //google drive link to meta data

    [Header("File Data")]
    public string associatedFileLink; //drive link for actual file
    public string associatedFileExtension; //extension of the file
    public string version;

    //runtime
    public string LocalMetadataFilePath => Path.Combine(Application.streamingAssetsPath, fileName + extension); //expression body

    public string LocalFilePath => Path.Combine(Application.streamingAssetsPath, fileName + associatedFileExtension);

    public string DirectMetadataDownloadLink => GoogleDriveStorage.ConvertToDirectDownloadLink(metadataFileLink);

    public string RemoteFileDownloadLink { get { return GoogleDriveStorage.ConvertToDirectDownloadLink(associatedFileLink); } set { } }

    //check for local meta data and delete if needed
    public void SetupLocalMetaData()
    {
        version = string.Empty;

        if (File.Exists(LocalMetadataFilePath))
        {
            // if we are here there is local meta data
            string localMetaDataContent = File.ReadAllText(LocalMetadataFilePath).ToString();
            MetaDataFile localMetaData = JsonUtility.FromJson<MetaDataFile>(localMetaDataContent);

            if(localMetaData != null)
            {
                version = localMetaData.version;
            }
            //Delete here because we have local version, need to check online
            File.Delete(LocalMetadataFilePath);
        }
        else
        {
            //No previous data
            version = "-1";
        }
    }

    public bool FileNeedsUpdating()
    {
        Debug.Log("Checking Local metadata file at path: " + LocalMetadataFilePath);

        //check if exists
        if (File.Exists(LocalMetadataFilePath) )
        {
            Debug.Log("Metadata file does not exist, update needed");
            return true;
        }

        string metadataContent = File.ReadAllText(LocalMetadataFilePath);

        //Read data
        if (string.IsNullOrEmpty(metadataContent))
        {
            Debug.Log("metadata file is empty, update needed");
            return true;
        }

        //Parse data
        MetaDataFile remoteMetaData = JsonUtility.FromJson<MetaDataFile>(metadataContent);
        if (remoteMetaData == null)
        {
            Debug.LogError("Failed to Parse metadata content, update needed");
            return true;
        }

        //compare versions
        if(version != remoteMetaData.version)
        {
            Debug.Log($"new version detected: {remoteMetaData.version}. updating from {version}");
            version = remoteMetaData.version;

            return true;   
        }

        Debug.Log($"{fileName} is up to date");
        return false;
    }

    public void DeleteLocalFile()
    {
        if (File.Exists(LocalFilePath))
        {
            File.Delete(LocalFilePath);
            Debug.Log($"Deleting outdated file at {LocalFilePath}");
        }
    }
}
