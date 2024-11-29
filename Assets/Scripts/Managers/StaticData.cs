using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    //PLAYER VECTOR
    public static Transform playerPosition;

    //AUDIO HANDLER
    public static bool isMuted { get; set; }

    //NPC DATA HOLDER
    public static string npcName;
    public static int npcHealth;
    public static int npcAttackSpeed;
    public static string pcRequirement;
    public static string npcCorrectCode;

    //ENEMY DATA HOLDER
    public static string enemyName;
    public static int enemyAttackSpeed;
    public static int enemyTimeLimit;
    public static string enemyRequirement;
    public static string enemyRequirementDescription;
    public static string enemyCodeTemplate;
    public static string enemyCorrectCode;
    public static Sprite enemysprite;
    public static GameObject enemyBadge;
    public static AnimationClip characterAnimation;
    public static Dictionary<string, string> bestTime = new Dictionary<string, string>();

    //BADGE DATA HOLDER
    public static string badgeName;
    public static string badgeDescription;

    //BOOK DATA HOLDER
    public static string bookTitle;
    public static string bookContent;

    //STATUS FLAGS
    public static bool tutorialIsActive;
    public static StaticData Instance { get; private set; }

    private void Awake()
    {
        isMuted = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        AudioSource[] audiosources = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource audiosource in audiosources)
        {
            if (isMuted)
            {
                break;
            }
            audiosource.Play();
        }
        
    }
    public static void muteUnmute()
    {
        isMuted = !isMuted;

        AudioSource[] audiosources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audiosource in audiosources)
        {
            audiosource.mute = isMuted;

        }
    }
}
