using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    [SerializeField] private List<MetadataScriptableObject> filesToDownload = new List<MetadataScriptableObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckAndDownloadFiles());
    }

    private IEnumerator CheckAndDownloadFiles()
    {
        foreach(MetadataScriptableObject metadata in filesToDownload)
        {
            metadata.SetupLocalMetaData();
            yield return StartCoroutine(DownloadFile(metadata.DirectMetadataDownloadLink,metadata.LocalMetadataFilePath));
            yield return new WaitForEndOfFrame();

            if(metadata.FileNeedsUpdating())
            {
                metadata.DeleteLocalFile();
                //download new one
                if(!string.IsNullOrEmpty(metadata.LocalMetadataFilePath) && !string.IsNullOrEmpty(metadata.DirectMetadataDownloadLink))
                {
                    yield return StartCoroutine(DownloadFile(metadata.RemoteFileDownloadLink, metadata.LocalFilePath));
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    Debug.LogError("Failed to obtain a valid download link, or local meta data path is not valid");
                }
            }
            else
            {
                Debug.Log($"{metadata.fileName} is up to date");
            }

            yield return null;
        }
        yield return null;
    }

    private IEnumerator DownloadFile( string fileLink, string savePath)
    {
        if(string.IsNullOrEmpty(fileLink))
        {
            Debug.LogError("couldnt download file link");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(fileLink); //creates request to download

        yield return request.SendWebRequest(); //Start Downloading

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to download file: {request.error}");
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.WriteAllBytes(savePath, request.downloadHandler.data); //write the files to destination
            Debug.Log($"File Downloaded successfuly to: {savePath}");
        }
    }
}
