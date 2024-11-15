using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CodeChecking : MonoBehaviour
{
    private SceneLoader sceneController;
    public GameObject enemyObject;
    public GameObject playerObject;

    public GameObject instructionPanel;
    public TextMeshProUGUI requirementTitleUI;
    public TextMeshProUGUI requirementDescUI;
    public TextMeshProUGUI enemyNameUI;
    public TextMeshProUGUI enemyBestTimeUI;

    public GameObject codeCheckPanel;
    public TextMeshProUGUI resultTextUI;

    public GameObject winPanel;
    public GameObject losePanel;

    public TMP_InputField inputField;
    public TextMeshProUGUI correctCode;

    public TextMeshProUGUI timerUI;
    private int startTime;
    private float timeRemaining, timeSpent;
    private string enemyName, reqTitle, reqDesc, timeUI;
    public Sprite enemySpriteB;

    public GameObject pauseMenuPanel;

    private bool panelIsActive, pausepanelIsActive, codeCheckPanelIsActive, timerRunning;

    


    private void Start()
    {
        sceneController = FindObjectOfType<SceneLoader>();
        SpriteRenderer spriteRenderer = enemyObject.GetComponent<SpriteRenderer>();

        instructionPanel.SetActive(true);
        panelIsActive = true;

        codeCheckPanelIsActive = false;
        codeCheckPanel.SetActive(false);

        pausepanelIsActive = false;
        pauseMenuPanel.SetActive(false);

        timerRunning = false;

        enemySpriteB = StaticData.enemysprite;
        spriteRenderer.sprite = enemySpriteB;

        enemyName = StaticData.enemyName;
        reqTitle = StaticData.enemyRequirement;
        requirementTitleUI.SetText(reqTitle);
        reqDesc = StaticData.enemyRequirementDescription;
        requirementDescUI.SetText(reqDesc);

        timeUI = StaticData.enemyTimeLimit.ToString();  
        timerUI.SetText(timeUI);

        startTime = StaticData.enemyTimeLimit;
        timeRemaining = startTime;
        timeSpent = startTime - timeRemaining;

        if (StaticData.bestTime.ContainsKey(enemyName))
        {
            //display shortest time spent (best record)
            enemyNameUI.SetText(enemyName);
            enemyBestTimeUI.SetText("Best Time: " + StaticData.bestTime[enemyName]);
        }
        else
        {
            enemyNameUI.SetText(enemyName);
            enemyBestTimeUI.SetText("Best Time: " + timeRemaining.ToString());
        }

        string template = StaticData.enemyCodeTemplate;
        inputField.text = template;

        string enemySourceCodeData = StaticData.enemyCorrectCode;
        correctCode.SetText(enemySourceCodeData);
        //correctCode.enabled = false;
    }
    private void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                // Decrement time and update text every frame
                timeRemaining -= Time.deltaTime;
                timerUI.text = Mathf.FloorToInt(timeRemaining).ToString();
                timeSpent += Time.deltaTime;
                //Debug.Log(Mathf.FloorToInt(timeSpent).ToString());
            }
            else
            {
                // When timer hits zero, stop the countdown
                timeRemaining = 0;
                timerRunning = false;
                timerUI.text = "0";
                // Optionally, trigger other events here when timer reaches zero
                losePanel.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && panelIsActive) {
            instructionPanel.SetActive(false);
            panelIsActive = false;
            timerRunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !pausepanelIsActive && !panelIsActive && codeCheckPanelIsActive)
        {
            codeCheckPanel.SetActive(false);
            codeCheckPanelIsActive = false;
            resultTextUI.text = string.Empty;
            timerRunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !pausepanelIsActive && !panelIsActive)
        {
            timerRunning = false;
            pauseMenuPanel.SetActive(true);
            pausepanelIsActive = true;
        }
    }
    //private IEnumerable<string> EnumerateLines(TMP_Text text)
    //{
    //    var textInfo = text.GetTextInfo(text.text);
    //    for (int i = 0; i < textInfo.lineCount; i++)
    //    {
    //        TMP_LineInfo lineInfo = textInfo.lineInfo[i];
    //        int startIndex = lineInfo.firstCharacterIndex;
    //        int length = lineInfo.characterCount;
    //        // Ensure the substring is within the bounds of the text
    //        if (startIndex + length <= text.text.Length)
    //        {
    //            yield return text.text.Substring(startIndex, length);
    //        }
    //        else
    //        {
    //            // Handle the out-of-bounds case, e.g., log a warning or return an empty string
    //            Debug.LogWarning("Invalid line information: " + i);
    //            yield return string.Empty;
    //        }
    //    }
    //}
    public void compareCode()
    {
        // Start the comparison coroutine
        StartCoroutine(CompareCodeWithDelay());
    }
    private IEnumerator CompareCodeWithDelay() {

        string fullInputText = inputField.text;
        string fullCodeText = correctCode.text;

        string[] linesI = fullInputText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        string[] linesC = fullCodeText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        List<string> userInputLines = new List<string>();
        foreach (string line in linesI)
        {
            userInputLines.Add(line.Trim()); // Trim to remove leading/trailing whitespace
        }

        List<string> correctLines = new List<string>();
        foreach (string line in linesC)
        {
            correctLines.Add(line.Trim()); // Trim to remove leading/trailing whitespace
        }

        //Comparison Algorithm
        List<bool> isCorrect = new List<bool>();

        if (userInputLines.Count != correctLines.Count)
        {
            // Handle the case where the number of lines doesn't match
            resultTextUI.text += "Incorrect number of lines.";
            Debug.Log("Incorrect number of lines.");
            yield break;
        }

        for (int i = 0; i < userInputLines.Count; i++)        //compare input to correctlines
        {

            if (!userInputLines[i].Equals(correctLines[i], StringComparison.OrdinalIgnoreCase))
            {
                isCorrect.Add(false);
                if (i == 0) 
                {
                    resultTextUI.text += "Line (" + (i + 1) + ") is incorrect : " + userInputLines[i];
                }
                resultTextUI.text += "\nLine (" + (i+1) + ") is incorrect : " + userInputLines[i];
                yield break;
            }
            else
            {
                isCorrect.Add(true);
                if (i == 0)
                {
                    resultTextUI.text += "Line (" + (i + 1) + ") is correct";
                }
                resultTextUI.text += "\nLine (" + (i + 1) + ") is correct";
            }
            yield return new WaitForSeconds(0.5f);
        }

        // Display the result
        if (isCorrect.All(x => x))
        {
            winPanel.SetActive(true);
            if (StaticData.bestTime.ContainsKey(enemyName))
            {
                if (Mathf.FloorToInt(timeSpent) < int.Parse(StaticData.bestTime[enemyName]))
                {
                    StaticData.bestTime[enemyName] = Mathf.FloorToInt(timeSpent).ToString();
                }
            }
            else
            {
                StaticData.bestTime.Add(enemyName, Mathf.FloorToInt(timeSpent).ToString());       //Add new record to dictionary
                Debug.Log("New Record meega!");
            }
        }
        else
        {
            Debug.Log("Code is incorrect.");
        }
    }
    public void OnCheckButtonPressed() {
        if (!codeCheckPanelIsActive)
        {
            timerRunning = false;
            codeCheckPanelIsActive = true;
            codeCheckPanel.SetActive(true);
            Invoke("compareCode", 1.0f);
            //compareCode();
        }
    }
    public void OnResetCodePressed()
    {
        string template = StaticData.enemyCodeTemplate;
        inputField.text = template;
    }
    public void OnShowInstructionPressed()
    {
        if (!panelIsActive) {
            instructionPanel.SetActive(true);
            panelIsActive = true;
        }
        else { 
            instructionPanel.SetActive(false);
            panelIsActive = false;
        }
    }
    public void OnMenuPressed()
    {
        if (!pausepanelIsActive)
        {
            pauseMenuPanel.SetActive(true);
            pausepanelIsActive = true;
        }
        else
        {
            pauseMenuPanel.SetActive(false);
            pausepanelIsActive = false;
        }
    }
    public void OnResumePressed()
    {
        if (pausepanelIsActive)
        {
            pauseMenuPanel.SetActive(false);
            pausepanelIsActive = false;
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            pausepanelIsActive = true;
        }
    }
    public void OnRetryPressed()
    {
        SceneManager.UnloadSceneAsync("BattleScene");
        sceneController.TransitionToBattleScene();
    }
    public void OnReturnPressed()
    {
        sceneController.ReturnToMainScene();
    }
    public void OnHomePressed()
    {
        if (pausepanelIsActive)
        {
            sceneController.ReturnToMainScene();
        }
    }
}
