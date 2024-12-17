using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using Newtonsoft.Json;

[Serializable]
class SaveData
{
    public float posX;
    public float posY;
}

public class GameSaveManager : MonoBehaviour
{
    //Singleton Setup
    public static GameSaveManager Instance { get; private set; }
    private string saveUrl = "http://localhost/ByteQuestWeb/save_player_position.php";
    private string loadUrl = "http://localhost/ByteQuestWeb/load_player_position.php";
    public Vector3 playerPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");

        SaveData data = new SaveData();
        data.posX = playerPosition.x;
        data.posY = playerPosition.y;

        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Game Data Saved:  "+ data.posX.ToString() + data.posY.ToString());

        // Save to PlayFab
        StartCoroutine(SaveDataToDatabase(data));
        //SavePlayerPositionToPlayFab(data);
    }

    IEnumerator SaveDataToDatabase(SaveData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", UserSessionManager.Instance.LoggedInUsername);
        form.AddField("posX", data.posX.ToString());
        form.AddField("posY", data.posY.ToString());

        UnityWebRequest www = UnityWebRequest.Post(saveUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Position saved: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error saving position: " + www.error);
        }
    }


    public void LoadGame()
    {
        StartCoroutine(LoadDataFromDatabase());
        //if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        //{
        //    // Load locally
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
        //    SaveData data = (SaveData)bf.Deserialize(file);
        //    file.Close();

        //    playerPosition = new Vector3(data.savedPlayerPositionX, data.savedPlayerPositionY, playerPosition.z);
        //    SetPlayerPosition();
        //    Debug.Log("Game Data Loaded Locally");
        //}
        //else
        //{
        //    Debug.LogWarning("No local save data found, attempting to load from PlayFab.");
        //    StartCoroutine(LoadDataFromDatabase());
        //    //LoadPlayerPositionFromPlayFab();
        //}

    }
    IEnumerator LoadDataFromDatabase()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", UserSessionManager.Instance.LoggedInUsername);
        Debug.Log(UserSessionManager.Instance.LoggedInUsername);
        UnityWebRequest www = UnityWebRequest.Post(loadUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            // Print the raw JSON response for debugging
            Debug.Log("Raw response from server: " + json);
            if (json == "No data found")
            {
                Debug.LogWarning("No player data found in the database.");
                yield break;
            }

            //SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Deserialized Position X: " + data.posX);
            Debug.Log("Deserialized Position Y: " + data.posY);
            playerPosition = new Vector3(data.posX, data.posY, 0);
            SetPlayerPosition();
            Debug.Log("Position loaded successfully.");
        }
        else
        {
            Debug.LogError("Error loading position: " + www.error);
        }
    }

    private void SetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Replace "Player" with your player's tag
        if (player != null)
        {
            player.transform.position = playerPosition;
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }

    //private void SavePlayerPositionToPlayFab(SaveData data)
    //{
    //    var positionData = new Dictionary<string, string>
    //    {
    //        { "PlayerPositionX", data.savedPlayerPositionX.ToString() },
    //        { "PlayerPositionY", data.savedPlayerPositionY.ToString() }
    //    };

    //    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
    //    {
    //        Data = positionData
    //    },
    //    result => Debug.Log("Player position saved to PlayFab."),
    //    error => Debug.LogError("Failed to save player position to PlayFab: " + error.GenerateErrorReport()));
    //}

    //private void LoadPlayerPositionFromPlayFab()
    //{
    //    PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
    //    result =>
    //    {
    //        if (result.Data.ContainsKey("PlayerPositionX") && result.Data.ContainsKey("PlayerPositionY"))
    //        {
    //            float x = float.Parse(result.Data["PlayerPositionX"].Value);
    //            float y = float.Parse(result.Data["PlayerPositionY"].Value);

    //            playerPosition = new Vector3(x, y, playerPosition.z);
    //            SetPlayerPosition();
    //            Debug.Log("Player position loaded from PlayFab.");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("No player position data found on PlayFab.");
    //        }
    //    },
    //    error => Debug.LogError("Failed to load player position from PlayFab: " + error.GenerateErrorReport()));
    //}
}
