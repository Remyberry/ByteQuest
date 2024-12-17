using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    //public GameObject[] enemyGameObjects;
    public GameObject enemyListParent;
    [System.Serializable]
    private class EnemyListWrapper
    {
        public List<Enemy> Enemies;
    }
    // Start is called before the first frame update
    void Start()
    {
        FetchEnemyDataFromPlayFab(); 
    }

    void FetchEnemyDataFromPlayFab()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceived, OnPlayFabError);
    }
    void OnTitleDataReceived(GetTitleDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("Enemies"))
        {
            // Parse the JSON string from PlayFab Title Data
            string enemyJson = result.Data["Enemies"];
            //List<Enemy> enemies = JsonUtility.FromJson<EnemyListWrapper>(enemyJson).Enemies;
            List<Enemy> enemies = JsonConvert.DeserializeObject<List<Enemy>>(enemyJson);
            // Assign enemies to GameObjects
            AssignEnemyDataToGameObjects(enemies);
        }
        else
        {
            Debug.LogError("No 'Enemies' key found in PlayFab Title Data.");
        }
    }

    void AssignEnemyDataToGameObjects(List<Enemy> enemies)
    {
        foreach (Transform enemyTransform in enemyListParent.transform)
        {
            string enemyName = enemyTransform.name;

            // Find the matching enemy data
            Enemy enemyData = enemies.Find(e => e.enemyName == enemyName);

            if (enemyData != null)
            {
                // Get the Trigger GameObject and EnemyTrigger script
                var triggerObject = enemyTransform.Find("Trigger");
                if (triggerObject != null)
                {
                    EnemyTrigger triggerScript = triggerObject.GetComponent<EnemyTrigger>();
                    if (triggerScript != null)
                    {
                        triggerScript.enemy = enemyData; // Assign the enemy data
                        Debug.Log($"Assigned data to {enemyName}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Trigger GameObject not found for {enemyName}");
                }
            }
            else
            {
                Debug.LogWarning($"No matching enemy data found for {enemyName}");
            }
        }
    }

    void OnPlayFabError(PlayFabError error)
    {
        Debug.LogError("Error fetching data from PlayFab: " + error.GenerateErrorReport());
    }

    
}
