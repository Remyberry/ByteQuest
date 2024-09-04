using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputField;
    public TextMeshProUGUI[] lineTextFields; // Array to store TextMeshProUGUI components for each line
    public string[] expectedLines; // Array of expected lines for the current level

    private string[] userInputLines;

    void Start()
    {
        /*// Initialize the expected lines array with the correct code for the current level
        expectedLines = new string[] { "mov ax, 18A3h", "int 21h" };

        // Create and initialize line text fields (adjust as needed based on your UI)
        lineTextFields = new TextMeshProUGUI[expectedLines.Length];
        for (int i = 0; i < lineTextFields.Length; i++)
        {
            lineTextFields[i] = *//* Instantiate a TextMeshProUGUI object and assign it here *//*;
        }*/
    }
    public void OnInputChange()
    {
        // Split user input into lines
        userInputLines = inputField.text.Split('\n');

        // Check each line against the expected lines
        for (int i = 0; i < userInputLines.Length; i++)
        {
            bool isCorrect = userInputLines[i].Equals(expectedLines[i], StringComparison.OrdinalIgnoreCase);
            lineTextFields[i].color = isCorrect ? Color.green : Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
