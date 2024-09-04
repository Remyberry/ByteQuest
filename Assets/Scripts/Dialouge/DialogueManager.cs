using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField]  private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;
    public static DialogueManager instance;
    private const string SPEAKER_TAG = "speaker";

    private DialogueVariables dialogueVariables;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariables();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        
        choicesText =  new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) 
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        if ((currentStory.currentChoices.Count == 0) && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Return)) && canContinueToNextLine) {
            ContinueStory();
        }

    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        nameText.text = "???";
        ContinueStory();
    }

    private void ExitDialogueMode() 
    {
        dialogueVariables.StartListening(currentStory);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {

        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private  IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        //letter displaying
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
            //check if submitted again
            
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length == 2)
            {
                Debug.LogError("Tag could not be appropriately parsed " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    nameText.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag came in but not handled" + tag);
                    break;
            }
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //checks if UI cant support number of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        //enable UI for choices
        int index = 0;
        foreach (Choice choice in currentChoices) {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        choices[0].gameObject.GetComponent<Button>().Select();
        //set ramaining UI choices inactive
        for (int i = index; i < choices.Length; i++) 
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.gameObject.SetActive(false);
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
        }
    }

}
