using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using Unity.VisualScripting;
using UnityEngine.Device;
using SystemInfo = UnityEngine.SystemInfo;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField emailUI;
    public TMP_InputField passwordUI;
    public TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Start()
    {
        if (StaticData.isLoggedIn)
        {
            loginPanel.SetActive(false);
        }
    }
    public void LoginInfo()
    {
        LoginWithEmail(emailUI.text, passwordUI.text);
    }
    public void RegisterInfo()
    {
        if (passwordUI.text.Length < 6)
        {
            messageText.text = "Password must have at least 6 characters!";
            passwordUI.text = "";
            emailUI.text = "";
            return;
        }
        RegisterAccount(emailUI.text, passwordUI.text);
    }
    public void ResetPasswordButton()
    {
        ResetPassword(emailUI.text);
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    // User Login
    void LoginWithEmail(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult result){
        messageText.text = "Successful login!";
        Debug.Log("Successful login!");
        StaticData.isLoggedIn = true;
        loginPanel.SetActive(false);
    }

    void OnLoginFailure(PlayFabError error){
        messageText.text = "Error while logging in!"; 
        Debug.Log(error.GenerateErrorReport());
    }

    //UserRergister     =============================================================================
    void RegisterAccount(string email, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Account created successfully!";
        Debug.Log("Account created successfully!");
        loginPanel.SetActive(false);
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Account registration failed: " + error.GenerateErrorReport());
    }

    void ResetPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest{
            Email = email,
            TitleId = "A3AB5"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Reset password sent to email!";
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
