using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [Header("Register")]
    public GameObject registerPanel;
    public TMP_InputField usernameInputR;  // New input for registration
    public TMP_InputField emailInputR;     
    public TMP_InputField passwordInputR;
    public TextMeshProUGUI feedbackText;

    private string loginURL = "http://localhost/ByteQuestWeb/login.php";
    private string registerURL = "http://localhost/ByteQuestWeb/register.php";


    void Start()
    {
        if (StaticData.isLoggedIn)
        {
            loginPanel.SetActive(false);
        }
    }
    // Register Method
    public void Register()
    {
        StartCoroutine(RegisterRequest(usernameInputR.text, emailInputR.text, passwordInputR.text));
    }

    IEnumerator RegisterRequest(string username, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Contains("Registration successful"))
                {
                    feedbackText.text = "Registration Successful!";
                    registerPanel.SetActive(false);
                }
                else
                {
                    feedbackText.text = "Registration Failed: " + response;
                }
            }
            else
            {
                feedbackText.text = "Error: " + www.error;
            }
        }
    }

    // Login Method
    public void Login()
    {
        StartCoroutine(LoginRequest(usernameInput.text, passwordInput.text));
    }

    IEnumerator LoginRequest(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Contains("Login successful"))
                {
                    UserSessionManager.Instance.SetUsername(username);
                    feedbackText.text = "Login Successful!";
                    StaticData.isLoggedIn = true;
                    loginPanel.SetActive(false);
                }
                else
                {
                    feedbackText.text = "Login Failed: " + response;
                }
            }
            else
            {
                feedbackText.text = "Error: " + www.error;
            }
        }
    }


    // PlayerSave     =============================================================================
    void SavePlayerData(string key, string value)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { key, value }
        }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataUpdateSuccess, OnDataUpdateFailure);
    }

    void OnDataUpdateSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Player data saved successfully!");
    }

    void OnDataUpdateFailure(PlayFabError error)
    {
        Debug.LogError("Error saving data: " + error.GenerateErrorReport());
    }

    //PlayerLoad
    void LoadPlayerData(string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(key))
            {
                Debug.Log($"Loaded data: {key} = {result.Data[key].Value}");
            }
            else
            {
                Debug.LogWarning($"No data found for key: {key}");
            }
        }, error =>
        {
            Debug.LogError("Error loading data: " + error.GenerateErrorReport());
        });
    }

    //UpdateLeaderboard
    void UpdateScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "HighScore", Value = score }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            Debug.Log("Score updated successfully!");
        }, error =>
        {
            Debug.LogError("Error updating score: " + error.GenerateErrorReport());
        });
    }

    //GetLeaderBoard
    void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            foreach (var entry in result.Leaderboard)
            {
                Debug.Log($"{entry.Position}: {entry.DisplayName} - {entry.StatValue}");
            }
        }, error =>
        {
            Debug.LogError("Error retrieving leaderboard: " + error.GenerateErrorReport());
        });
    }

    //SaveInventory
    void SaveInventory(List<string> items)
    {
        var inventoryData = string.Join(",", items);
        SavePlayerData("Inventory", inventoryData);
    }

    //LoadInventory
    void LoadInventory()
    {
        LoadPlayerData("Inventory");
    }
}
