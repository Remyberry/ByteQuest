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
    public void OnSavePress()
    {
        Debug.Log("Code saving here");
    }

}
