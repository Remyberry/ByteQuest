using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BadgeSO", menuName = "ScriptableObjects/BadgeSO")]
public class Badge : ItemObject
{
    public string badgeName;
    [TextArea(5, 10)]
    public string badgeDescription;
    public void Awake()
    {
        type = ItemType.Badge;
    }
}
