using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeMan : MonoBehaviour
{
    public TextMeshProUGUI Header;
    public TextMeshProUGUI Content;
    private Queue<string> paragraphs = new();
    private Stack<string> displayedParagraphs = new();

    public void StartDialogue (Dialogue dialogue)
    {
        //for some reason, "paragraphs" wants reinitialization to work
        /*paragraphs = new Queue<string>();
        displayedParagraphs = new Stack<string>();*/

        Header.text = dialogue.name;
        Debug.Log(dialogue.name);
        paragraphs.Clear();

        foreach (string paragraph in dialogue.paragraphs)
        {
            paragraphs.Enqueue(paragraph);
        }
        DisplayNextParagraph();
    }

    public void DisplayNextParagraph()
    {
        Debug.Log(paragraphs.Count);
        if (paragraphs.Count == 0)
        {
            EndDialogue();
            return;
        }
        string paragraph = paragraphs.Dequeue();
        Content.text = paragraph;
        displayedParagraphs.Push(paragraph);
    }
    void EndDialogue()
    {
        Debug.Log("End");
    }

    public void DisplayPreviousParagraph() // New method to view previous
    {
        if (displayedParagraphs.Count == 0)
        {
            Debug.Log("No previous paragraphs to display");
            return;
        }

        string previousParagraph = displayedParagraphs.Pop();
        Content.text = previousParagraph;
    }
}
