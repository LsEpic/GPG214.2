using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class LoadFromAssetBundle : MonoBehaviour
{

    string folderPath = "AssetBundles";
    string fileName = "ChomperBundle";
    string combinedPath;

    private AssetBundle chomperEnemyBundle;

    // Start is called before the first frame update
    void Start()
    {
        LoadAssetBundle();
    }

    void LoadChomper()
    {
        if(chomperEnemyBundle == null)
        {
            return;
        }

        chomperEnemyBundle.LoadAllAssets();
    }

    void LoadAssetBundle()
    {
        combinedPath = Path.Combine(Application.streamingAssetsPath, folderPath, fileName);

        if (File.Exists(combinedPath))
        {
            chomperEnemyBundle = AssetBundle.LoadFromFile(combinedPath);
            Debug.Log("Asset Bundle Loaded");
        }
        else
        {
            Debug.Log("File doesnt Exist");
        }
    }
}
