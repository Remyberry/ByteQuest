using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "ScriptableObjects/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Books;
    public Dictionary<ItemObject, int> GetBookId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetBook = new Dictionary<int, ItemObject>();

    public ItemObject[] Badges;
    public Dictionary<ItemObject, int> GetBadgeId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetBadge = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        GetBookId = new Dictionary<ItemObject, int>();
        GetBook = new Dictionary<int, ItemObject>();
        for (int i = 0; i < Books.Length; i++)
        {
            GetBookId.Add(Books[i], i);
            GetBook.Add(i, Books[i]);
        }

        GetBadgeId = new Dictionary<ItemObject, int>();
        GetBadge = new Dictionary<int, ItemObject>();
        for (int i = 0; i < Badges.Length; i++)
        {
            GetBadgeId.Add(Badges[i], i);
            GetBadge.Add(i, Badges[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
