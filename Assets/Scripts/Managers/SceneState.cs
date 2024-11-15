using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SceneState : MonoBehaviour
{
    public List<GameObject> objectsToSave;

    public void SaveSceneState()
    {
        var state = new List<ObjectState>();

        foreach (var obj in objectsToSave)
        {
            var objState = new ObjectState
            {
                Position = obj.transform.position,
                IsActive = obj.activeSelf,
                Name = obj.name
            };
            state.Add(objState);
        }

        File.WriteAllText(Application.persistentDataPath + "/sceneState.json", JsonConvert.SerializeObject(state));
    }

    public void LoadSceneState()
    {
        if (File.Exists(Application.persistentDataPath + "/sceneState.json"))
        {
            var state = JsonConvert.DeserializeObject<List<ObjectState>>(File.ReadAllText(Application.persistentDataPath + "/sceneState.json"));

            foreach (var objState in state)
            {
                var obj = objectsToSave.Find(o => o.name == objState.Name);
                if (obj != null)
                {
                    obj.transform.position = objState.Position;
                    obj.SetActive(objState.IsActive);
                }
            }
        }
    }
}
[System.Serializable]
public class ObjectState
{
    public Vector3 Position;
    public bool IsActive;
    public string Name;
}