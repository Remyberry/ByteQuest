using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string enemyName;
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

    public Enemy(string enemyName, int attackSpeed, int timeLimit, string requirement, string requirementDescription, string codeTemplate, string correctCode)
    {
        this.enemyName = enemyName;
        this.attackSpeed = attackSpeed;
        this.timeLimit = timeLimit;
        this.requirement = requirement;
        this.requirementDescription = requirementDescription;
        this.codeTemplate = codeTemplate;
        this.correctCode = correctCode;

    }
}
