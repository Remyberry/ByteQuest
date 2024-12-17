using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataManager : MonoBehaviour
{
    private string saveUrl = "http://localhost/ByteQuestWeb/save_player_data.php";
    private string loadUrl = "http://localhost/ByteQuestWeb/load_player_data.php";

    public string username = UserSessionManager.Instance.LoggedInUsername;
    public Vector3 playerPosition;
    public int bestTime;

    // Save Player Data
    public void SavePlayerData()
    {
        StartCoroutine(SaveDataCoroutine());
    }

    IEnumerator SaveDataCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("posX", playerPosition.x.ToString());
        form.AddField("posY", playerPosition.y.ToString());
        form.AddField("bestTime", bestTime.ToString());

        UnityWebRequest www = UnityWebRequest.Post(saveUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data Saved: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error Saving Data: " + www.error);
        }
    }

    // Load Player Data
    public void LoadPlayerData()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    IEnumerator LoadDataCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post(loadUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;

            if (json == "No data found")
            {
                Debug.LogWarning("No player data found.");
                yield break;
            }

            // Parse JSON data
            //PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            
            playerPosition = new Vector3(data.posX, data.posY, 0);
            bestTime = data.bestTime;

            Debug.Log("Data Loaded Successfully");
        }
        else
        {
            Debug.LogError("Error Loading Data: " + www.error);
        }
    }

    [System.Serializable]
    private class PlayerData
    {
        public float posX;
        public float posY;
        public int bestTime;
    }
}
