using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTrigger : MonoBehaviour
{
    private SceneLoader sceneController;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private string sceneToLoad;
    private Transform playerPosition;
    public Enemy enemy;
    [SerializeField] public GameObject enemyBadge;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start()
    {
        sceneController = FindObjectOfType<SceneLoader>();
        playerPosition = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                string name = enemy.enemyName;
                int attackSpeed = enemy.attackSpeed;
                int timeLimit = enemy.timeLimit;
                string requirement = enemy.requirement;
                string requirementDesc = enemy.requirementDescription;
                string codeTemp = enemy.codeTemplate;
                string correctCode = enemy.correctCode;

                StaticData.enemyName = name;
                StaticData.enemyAttackSpeed = attackSpeed;
                StaticData.enemyTimeLimit = timeLimit;
                StaticData.enemyRequirement = requirement;
                StaticData.enemyRequirementDescription = requirementDesc;
                StaticData.enemyCodeTemplate = codeTemp;
                StaticData.enemyCorrectCode = correctCode;

                SpriteRenderer parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
                StaticData.enemysprite = parentSpriteRenderer.sprite;

                //sceneController.TransitionToBattleScene();
                if (!StaticData.tutorialIsActive)
                {
                    GameSaveManager.Instance.playerPosition = playerPosition.position;
                    GameSaveManager.Instance.SaveGame();
                    SceneManager.LoadScene(sceneToLoad);
                }
                else {
                    SceneManager.LoadScene(sceneToLoad);
                }
                
            }

            //Check if player is victorious in last fight
            if (string.IsNullOrEmpty(StaticData.enemyName))             
            {
                return;
            }
            else
            {
                if ((enemyBadge != null) && StaticData.bestTime.ContainsKey(StaticData.enemyName))
                {
                    enemyBadge.SetActive(true);
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player in range!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Exit!");
        }
    }
}
