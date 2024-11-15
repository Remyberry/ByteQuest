using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    //NPC DATA HOLDER
    public static string npcName;
    public static int npcHealth;
    public static int npcAttackSpeed;
    public static string pcRequirement;
    public static string npcCorrectCode;

    //ENEMY DATA HOLDER
    public static string enemyName;
    public static int enemyHealth;
    public static int enemyAttackSpeed;
    public static int enemyTimeLimit;
    public static string enemyRequirement;
    public static string enemyRequirementDescription;
    public static string enemyCodeTemplate;
    public static string enemyCorrectCode;
    public static Sprite enemysprite;
    public static AnimationClip characterAnimation;

    public static Dictionary<string, string> bestTime = new Dictionary<string, string>();

    //BOOK DATA HOLDER
    public static string bookTitle;
    public static string bookContent;

    //STATUS FLAGS
    public static bool doorOneOpened;
}
