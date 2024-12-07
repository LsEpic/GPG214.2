using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MetadataScriptableObject))]
public class MetaDataScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MetadataScriptableObject metadata = (MetadataScriptableObject)target;

        if(GUILayout.Button("Export to Json"))
        {
            ExportToJson(metadata);
            EditorUtility.SetDirty(metadata); //somethings change so update and save changes
        }
    }

    private void ExportToJson(MetadataScriptableObject metadata)
    {
        MetaDataFile metadadataFile = new MetaDataFile
        {
            version = metadata.version,
            fileLink = metadata.associatedFileLink,
        };

        string json = JsonUtility.ToJson(metadadataFile,true);

        Directory.CreateDirectory(Application.streamingAssetsPath);

        File.WriteAllText(metadata.LocalMetadataFilePath, json);

        Debug.Log($"Metadata Exported to json at: {metadata.LocalMetadataFilePath}");
    }
}
