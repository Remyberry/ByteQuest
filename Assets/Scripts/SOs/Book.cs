using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookSO", menuName = "ScriptableObjects/BookSO")]
public class BookSO : ItemObject
{
    public string title;
    [TextArea(10,20)]
    public string content;
    public void Awake()
    {
        type = ItemType.Book;
    }
}
