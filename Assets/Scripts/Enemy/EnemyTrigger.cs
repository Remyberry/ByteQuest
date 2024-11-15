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

    public Enemy enemy;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start()
    {
        sceneController = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.I))
            {
                string name = enemy.enemyName;
                int health = enemy.health;
                int attackSpeed = enemy.attackSpeed;
                int timeLimit = enemy.timeLimit;
                string requirement = enemy.requirement;
                string requirementDesc = enemy.requirementDescription;
                string codeTemp = enemy.codeTemplate;
                string correctCode = enemy.correctCode;

                StaticData.enemyName = name;
                StaticData.enemyHealth = health;
                StaticData.enemyAttackSpeed = attackSpeed;
                StaticData.enemyTimeLimit = timeLimit;
                StaticData.enemyRequirement = requirement;
                StaticData.enemyRequirementDescription = requirementDesc;
                StaticData.enemyCodeTemplate = codeTemp;
                StaticData.enemyCorrectCode = correctCode;

                SpriteRenderer parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
                StaticData.enemysprite = parentSpriteRenderer.sprite;

                sceneController.TransitionToBattleScene();
                //SceneManager.LoadScene(sceneToLoad);
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
