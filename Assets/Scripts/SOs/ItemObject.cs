using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Key,
    Book,
    QuestItem,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject gameobject;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
