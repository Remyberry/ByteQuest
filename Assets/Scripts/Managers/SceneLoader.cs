using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SceneLoader : MonoBehaviour
{
    public Scene mainMenu;
    public Scene mainScene;
    public Scene battleScene;

    private void Start()
    {
        mainScene = SceneManager.GetActiveScene();
    }
    public void LoadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TransitionToBattleScene()
    {
        // Disable main scene objects
        ToggleMainSceneObjects(false);

        // Load the battle scene additively
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }

    public void ReturnToMainScene()
    {
        if (battleScene != null && battleScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("BattleScene");
        }
        else
        {
            Debug.LogWarning("Battle scene is not loaded or assigned.");
        }

        ToggleMainSceneObjects(true);
    }

    private void ToggleMainSceneObjects(bool isActive)
    {
        if (mainScene == null)
        {
            Debug.LogError("Main scene is not assigned.");
            return;
        }

        foreach (GameObject obj in mainScene.GetRootGameObjects())
        {
            obj.SetActive(isActive);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            battleScene = scene;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
