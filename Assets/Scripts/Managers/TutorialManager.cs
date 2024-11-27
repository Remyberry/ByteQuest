using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    //[SerializeField] private GameObject [] tutorials;
    public List<GameObject> tutorials = new List<GameObject>();
    public bool tutorialIsDestroyed { get; private set; }
    private HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>(); // To track pressed keys
    private HashSet<KeyCode> requiredKeys = new HashSet<KeyCode>
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
    };
    private HashSet<KeyCode> requiredKeys2 = new HashSet<KeyCode>
    {
        KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
    };
    private void Awake()
    {
        tutorialIsDestroyed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (tutorialIsDestroyed)
        {
            return;
        }
        else 
        {
            foreach (KeyCode key in requiredKeys)
            {
                if (Input.GetKeyDown(key) && !pressedKeys.Contains(key))
                {
                    pressedKeys.Add(key);
                    Debug.Log($"Key pressed: {key}");
                }
            }
            foreach (KeyCode key in requiredKeys2)
            {
                if (Input.GetKeyDown(key) && !pressedKeys.Contains(key))
                {
                    pressedKeys.Add(key);
                    Debug.Log($"Key pressed: {key}");
                }
            }
            // Check if all required keys have been pressed
            if ((pressedKeys.Count == requiredKeys.Count) || (pressedKeys.Count == requiredKeys2.Count))
            {
                GameObject tutorial = tutorials[0];
                tutorials.RemoveAt(0);
                tutorialIsDestroyed = true;
                Destroy(tutorial);
            }
        }
        
    }

}
