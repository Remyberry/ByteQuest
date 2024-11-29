using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.Device;
using SystemInfo = UnityEngine.SystemInfo;

public class PlayFabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    // User Login
    void LoginWithEmail(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    void Login() {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    void OnLoginSuccess(LoginResult result){
        Debug.Log("Successful login/account create!");
    }
       
    void OnLoginFailure(PlayFabError error){
        Debug.Log("Error while logging in/creating account!"); 
        Debug.Log(error.GenerateErrorReport());
    }

    //UserRergister
    public void RegisterAccount(string email, string username, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Username = username,
            Password = password
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Account created successfully! Welcome, " + result.Username);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Account registration failed: " + error.GenerateErrorReport());
    }

    // PlayerSave
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
