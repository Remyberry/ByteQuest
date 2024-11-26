using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCTrigger : MonoBehaviour
{
    [Header("NPC Name")]
    [SerializeField] public NPC npc;
    [Header("Book")]
    [SerializeField] public GameObject bookItem;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (bookItem != null) {
                bookItem.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.I)) {
                string npcName = npc.name;
                StaticData.npcName = npcName;
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                Debug.Log("Key pressed I");
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            Debug.Log("NPC in range!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            Debug.Log("NPC out of range!");
        }
    }
}
