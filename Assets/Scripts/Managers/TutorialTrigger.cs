using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public GameObject tutorialPanel; // Assign the tutorial panel in the Inspector
    public TextMeshProUGUI tutorialText;
    public string tutorialDescription;
    public string requiredTag = "Player"; // Tag to identify the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Ey");
            ShowTutorialPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HideTutorialPanel();
        }
    }
    private void ShowTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = tutorialDescription;
            Debug.Log("Tutorial panel shown.");
        }
    }

    private void HideTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            tutorialText.text = "";
            Debug.Log("Tutorial panel hidden.");
        }
    }
}
