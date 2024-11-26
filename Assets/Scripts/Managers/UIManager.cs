using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button saveButton;
    public GameObject savepanel;
    private Transform playerPosition;

    public bool pauseisActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one ui manager in the scene");
        }
        instance = this;
    }
    public static UIManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        pauseisActive = false;
        pauseOverlay.SetActive(false);
        playerPosition = FindObjectOfType<Player>().transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pauseisActive = true;
            pauseOverlay.SetActive(true);
            resumeButton.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnMenuPress()
    {
        if (DialogueManager.instance.dialogueIsPlaying)
        {
            return;
        }
    }
    public void OnPausePress()
    {
        pauseOverlay.SetActive(true);
    }
    public void OnResumePress()
    {
        pauseisActive = false;
        pauseOverlay.SetActive(false);
    }
    public void OnHomePress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnSavePanelPress()
    {
        savepanel.SetActive(true);
    }
    public void OnSavePress()
    {
        GameSaveManager.Instance.playerPosition = playerPosition.position;
        GameSaveManager.Instance.SaveGame();
        Debug.Log(playerPosition.position);
    }
    public void OnLoadPress()
    {
        GameSaveManager.Instance.LoadGame();
    }
    public void OnCancelPress()
    {
        savepanel.SetActive(false);
    }
}
