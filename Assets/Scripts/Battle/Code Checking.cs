using PlayFab.ClientModels;
using PlayFab;
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
    private float timeRemaining, timeSpent, attackInterval, attackTimer;
    private string enemyName, reqTitle, reqDesc, timeUI;
    public Sprite enemySpriteB;
    public GameObject pauseMenuPanel;

    private bool instructionPanelIsActive, pausepanelIsActive, codeCheckPanelIsActive, timerRunning;


    private void Start()
    {
        sceneController = FindObjectOfType<SceneLoader>();
        SpriteRenderer spriteRenderer = enemyObject.GetComponent<SpriteRenderer>();

        instructionPanel.SetActive(true);
        instructionPanelIsActive = true;

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
        attackInterval = StaticData.enemyAttackSpeed;
        timeUI = StaticData.enemyTimeLimit.ToString();  
        timerUI.SetText(timeUI);

        startTime = StaticData.enemyTimeLimit;
        timeRemaining = startTime;
        timeSpent = startTime - timeRemaining;

        LoadBestTime(() =>
        {
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
        });

        

        string template = StaticData.enemyCodeTemplate;
        inputField.text = template;

        string enemySourceCodeData = StaticData.enemyCorrectCode;
        correctCode.SetText(enemySourceCodeData);

        //initialize hp code

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

                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0f)
                {
                    EnemyAttack();
                    attackTimer = attackInterval;
                }
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
        if (Input.GetKeyDown(KeyCode.Escape) && instructionPanelIsActive) {        //Close instruction panel and start timer
            instructionPanel.SetActive(false);
            instructionPanelIsActive = false;
            timerRunning = true;
            inputField.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !pausepanelIsActive && !instructionPanelIsActive && codeCheckPanelIsActive) //Close checking panel and resume timer
        {
            codeCheckPanel.SetActive(false);
            codeCheckPanelIsActive = false;
            resultTextUI.text = string.Empty;
            timerRunning = true;
            inputField.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !pausepanelIsActive && !instructionPanelIsActive)      //Open pause and stop timer
        {
            timerRunning = false;
            pauseMenuPanel.SetActive(true);   
            pausepanelIsActive = true;
            inputField.Select();
        }
    }
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
                //if (i == 0)
                //{
                    resultTextUI.text += "Line (" + (i + 1) + ") is incorrect : " + userInputLines[i] + "\n";
                //}
                //else {
                //    resultTextUI.text += "\nLine (" + (i + 1) + ") is incorrect : " + userInputLines[i];
                //}
                yield break;
            }
            else
            {
                isCorrect.Add(true);
                //if (i == 0)
                //{
                    resultTextUI.text += "Line (" + (i + 1) + ") is correct\n";
                //}
                //else
                //{
                //    resultTextUI.text += "\nLine (" + (i + 1) + ") is correct";
                //}
                
            }
            yield return new WaitForSeconds(0.5f);
        }

        // Display the result
        if (isCorrect.All(x => x))
        {
            winPanel.SetActive(true);

            if (StaticData.bestTime.ContainsKey(enemyName))
            {
                if (Mathf.FloorToInt(timeSpent) < int.Parse(StaticData.bestTime[enemyName]))        //Check if current time spent is less than best time
                {
                    StaticData.bestTime[enemyName] = Mathf.FloorToInt(timeSpent).ToString();        
                }
            }
            else
            {
                StaticData.bestTime.Add(enemyName, Mathf.FloorToInt(timeSpent).ToString());       //Add new record to dictionary
                Debug.Log("New Record meega!");
                SaveBestTime();
            }
            //string bestTimeJson = JsonUtility.ToJson(new SerializableDictionary<string, string>(StaticData.bestTime));
            //string bestTimeJson = JsonUtility.ToJson(StaticData.bestTime);
            
        }
        else
        {
            Debug.Log("Code is incorrect.");
        }
    }
    public void EnemyAttack()
    {
        string[] lines = inputField.text.Split('\n');

        if (lines.Length > 1)
        {
            int randomIndex = UnityEngine.Random.Range(0, lines.Length); // Pick a random line index
            lines[randomIndex] = ""; 

            inputField.text = string.Join("\n", lines); // Reconstruct the text
        }
    }

    public void SaveBestTime()
    {
        string bestTimeJson = JsonUtility.ToJson(new SerializableDictionary<string, string>(StaticData.bestTime));
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "PlayerBestTime", bestTimeJson }
        }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnError);
    }

    private void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("Best time data successfully saved to PlayFab.");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error saving data to PlayFab: " + error.GenerateErrorReport());
    }

    public void LoadBestTime(Action onLoadComplete = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("PlayerBestTime"))
            {
                string bestTimeJson = result.Data["PlayerBestTime"].Value;
                SerializableDictionary<string, string> deserializedBestTime =
                    JsonUtility.FromJson<SerializableDictionary<string, string>>(bestTimeJson);

                StaticData.bestTime = deserializedBestTime.ToDictionary();
                Debug.Log("Best time data successfully loaded from PlayFab.");
            }
            else
            {
                Debug.Log("No best time data found in PlayFab.");
            }

            onLoadComplete?.Invoke(); // Call the callback once the data is loaded
        },
    error =>
    {
        Debug.LogError("Error loading data from PlayFab: " + error.GenerateErrorReport());
        onLoadComplete?.Invoke(); // Still call the callback to avoid blocking
    });
        //PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataLoaded, OnError);
    }

    private void OnDataLoaded(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("PlayerBestTime"))
        {
            string bestTimeJson = result.Data["PlayerBestTime"].Value;
            SerializableDictionary<string, string> deserializedBestTime =
                JsonUtility.FromJson<SerializableDictionary<string, string>>(bestTimeJson);

            StaticData.bestTime = deserializedBestTime.ToDictionary();
            Debug.Log("Best time data successfully loaded from PlayFab.");
        }
    }


    public void OnCheckButtonPressed() {
        if (!codeCheckPanelIsActive)
        {
            timerRunning = false;
            codeCheckPanelIsActive = true;
            codeCheckPanel.SetActive(true);
            Invoke("compareCode", 1.0f);
        }
    }
    public void OnResetCodePressed()
    {
        string template = StaticData.enemyCodeTemplate;
        inputField.text = template;
    }
    public void OnShowInstructionPressed()
    {
        if (!instructionPanelIsActive) {
            instructionPanel.SetActive(true);
            instructionPanelIsActive = true;
        }
        else { 
            instructionPanel.SetActive(false);
            instructionPanelIsActive = false;
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
        SceneManager.LoadScene("BattleScene");
        //sceneController.TransitionToBattleScene();
    }
    public void OnReturnPressed()
    {
        //SceneManager.UnloadSceneAsync("BattleScene");
        if (StaticData.tutorialIsActive)
        {
            SceneManager.LoadScene("MainMenu");
            StaticData.tutorialIsActive = false;
        }
        else { SceneManager.LoadScene("TestScene"); }
        
        //sceneController.ReturnToMainScene();
    }
    public void OnHomePressed()
    {
        if (pausepanelIsActive)
        {
            Debug.Log(StaticData.tutorialIsActive);
            if (StaticData.tutorialIsActive)
            {
                SceneManager.LoadScene("MainMenu");
                StaticData.tutorialIsActive = false;
            }
            else {
                SceneManager.LoadScene("TestScene"); 
            }
            //SceneManager.UnloadSceneAsync("BattleScene");
            //sceneController.ReturnToMainScene();
        }
    }
}
[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        foreach (var kvp in dictionary)     //key value pair
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        var dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
        return dictionary;
    }
}