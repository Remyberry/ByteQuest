using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
class SaveData
{
    public float savedPlayerPositionX;
    public float savedPlayerPositionY;
}

public class GameSaveManager : MonoBehaviour
{
    //Singleton Setup
    public static GameSaveManager Instance { get; private set; }

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
        SaveGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");

        SaveData data = new SaveData();
        data.savedPlayerPositionX = playerPosition.x;
        data.savedPlayerPositionY = playerPosition.y;

        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Game Data Saved");
    }


    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            playerPosition = new Vector3(data.savedPlayerPositionX, data.savedPlayerPositionY, playerPosition.z);

            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Replace "Player" with your player's name

            if (player != null)
            {
                player.transform.position = GameSaveManager.Instance.playerPosition;
            }
            else
            {
                Debug.LogError("Player object not found.");
            }
            Debug.Log("Game Data Loaded");
        }
        else
        {
            playerPosition = new Vector3(0, 0, 0);
            Debug.LogError("There is no save data");
        }


    }

}
