using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetter : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetDataFromServer());
    }

    IEnumerator GetDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost/ByteQuestWeb/Getter.php");
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data retrieved successfully");
            Debug.Log(request.downloadHandler.text);
            byte[] resultData = request.downloadHandler.data;
        }
        else
        {
            Debug.LogError("Error retrieving data: " + request.error);
        }
    }
}
