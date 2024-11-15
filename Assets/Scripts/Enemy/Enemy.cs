using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public int health;
    public int attackSpeed;
    public int timeLimit;
    [Multiline]
    public string requirement;
    [Multiline]
    public string requirementDescription;
    [Multiline]
    public string codeTemplate;
    [Multiline]
    public string correctCode;
}
