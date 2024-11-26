using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Book,
    Badge,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject gameobject;
    public ItemType type;
}
