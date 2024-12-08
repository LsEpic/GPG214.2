

// Script stopped working after i exported unity package with SDKs so i removed them so evereything else ran and will put in user manual to install SDKs


//using Firebase;
//using Firebase.Database;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;

//public class FirebaseDatabase : MonoBehaviour
//{
//    [SerializeField] private SaveData saveData;
//    [SerializeField] private SaveManager saveManager;

//    private SaveData dataFromTheServer;
//    private Coroutine interactingWithDatabaseRoutine;

//    DatabaseReference databaseReference;

//    // Start is called before the first frame update
//    void Start()
//    {
//        saveData = SaveManager.Instance.saveData;


//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E) && interactingWithDatabaseRoutine == null)
//        {
//            interactingWithDatabaseRoutine = StartCoroutine(SavePlayerDataToServer());
//        }
//        if(Input.GetKeyDown(KeyCode.R) && interactingWithDatabaseRoutine == null)
//        {
//            interactingWithDatabaseRoutine = StartCoroutine(LoadPlayerDataFromServer());
//        }
//    }

//    void InitAndGetDatabaseReference()
//    {
//        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
//    }

//    IEnumerator SavePlayerDataToServer()
//    {
//        string jsonData = JsonUtility.ToJson(saveData);

//        Task sendJSon = databaseReference.Child("users").Child("PlayerSaveData").SetRawJsonValueAsync(jsonData);

//        while(!sendJSon.IsCompleted && !(sendJSon.IsFaulted || sendJSon.IsCanceled))
//        {
//            yield return null;
//        }

//        if(sendJSon.IsFaulted || sendJSon.IsCanceled)
//        {
//            Debug.LogError("Error saving data");
//            yield break;
//        }

//        Debug.Log("Game is Saved online");

//        interactingWithDatabaseRoutine = null;
//        yield return null;
//    }

//    IEnumerator LoadPlayerDataFromServer()
//    {
//        Task<DataSnapshot> userdata = databaseReference.Child("users").Child("PlayerSaveData").GetValueAsync();

//        while(!userdata.IsCompleted && !(userdata.IsFaulted || userdata.IsCanceled))
//        {
//            yield return null;
//        }

//        if(userdata.IsFaulted || userdata.IsCanceled)
//        {
//            Debug.LogError("couldnt load data");
//            yield break;
//        }

//        Debug.Log("Data retrieved");

//        DataSnapshot snapShotRetrieved = userdata.Result;

//        string returnedJson = snapShotRetrieved.GetRawJsonValue();

//        if(!string.IsNullOrEmpty(returnedJson))
//        {
//            dataFromTheServer = JsonUtility.FromJson<SaveData>(returnedJson);
//        }
//        else
//        {
//            Debug.LogError("No data");
//        }

//        interactingWithDatabaseRoutine = null;
//        yield return null;
//    }
//}