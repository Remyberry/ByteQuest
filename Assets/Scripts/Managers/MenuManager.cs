using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject bookListPanel;
    //[SerializeField] private GameObject itemListPanel;
    //[SerializeField] private GameObject questListPanel;
    //[SerializeField] private GameObject playerPanel;

    public bool menuIsActive { get; private set; }
    public bool extendedPanelIsActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one menu manager in the scene");
        }
        instance = this;
    }
    public static MenuManager GetInstance()
    {
        return instance;
    }
    void Start()
    {
        menuIsActive = false;
        extendedPanelIsActive = false;
        menuPanel.SetActive(false);
        bookListPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying) 
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            menuIsActive = true;
            menuPanel.SetActive(true);
            menuPanel.GetComponentInChildren<Button>().Select();
        }
        if (menuIsActive && Input.GetKeyDown(KeyCode.Escape) && !BookManager.GetInstance().bookViewIsActive)
        {
            OnExitPress();
        }
    }
    public void OnBookPress()
    {
        extendedPanelIsActive = true;
        bookListPanel.SetActive(true);
    }
    public void OnItemsPress()
    {
        Debug.Log("Items is pressed");
    }
    public void OnPlayerPress()
    {
        Debug.Log("Player is pressed");
    }
    public void OnQuestsPress()
    {
        Debug.Log("Quests is pressed");
    }
    public void OnExitPress()
    {
        menuIsActive = false;
        menuPanel.SetActive(false);
        extendedPanelIsActive = false;
        bookListPanel.SetActive(false);
    }
}
