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
    public Scene tutorialScene;
    //public bool tutorialIsActive { get; private set; }
    public static SceneLoader instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one menu manager in the scene");
        }
        instance = this;
    }
  
    public static SceneLoader GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        //tutorialScene = SceneManager.GetSceneByBuildIndex(1);
        //mainScene = SceneManager.GetSceneByBuildIndex(2);
        //battleScene = SceneManager.GetSceneByBuildIndex(3);

        GameObject player = GameObject.Find("Player"); // Replace "Player" with your player's name

        if (player != null)
        {
            player.transform.position = GameSaveManager.Instance.playerPosition;
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }
    public void LoadScene(string sceneName) 
    {
        if (sceneName == "Tutorial") 
        {
            StaticData.tutorialIsActive = true;
        }
        else
        {
            StaticData.tutorialIsActive = false;
        }
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
    public void ReturnToMainMenu()
    {
        if (tutorialScene != null && tutorialScene.isLoaded)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("Tutorial scene is not loaded or assigned.");
        }
    }
    public void ExitGame()
    {
        Application.Quit();
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
        if (scene.name == "Tutorial")
        {
            StaticData.tutorialIsActive = true;
            tutorialScene = scene;
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
