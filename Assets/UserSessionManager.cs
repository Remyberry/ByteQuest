using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSessionManager : MonoBehaviour
{
    public static UserSessionManager Instance { get; private set; }

    public string LoggedInUsername { get; private set; } // Store username

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUsername(string username)
    {
        LoggedInUsername = username;
        Debug.Log("Username stored: " + LoggedInUsername);
    }

}
