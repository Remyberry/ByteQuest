using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayItems : MonoBehaviour
{
    public InventoryObject inventory;
    public BookSO bookSO;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        //CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        //TextMeshProUGUI[] textMeshProComponents = GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                if (inventory.Container[i].item.type ==  ItemType.Book) 
                {
                    BookSO book = (BookSO)inventory.Container[i].item;
                    itemsDisplayed[inventory.Container[i]].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = book.title;
                    //Debug.Log(book.title);

                }
                itemsDisplayed[inventory.Container[i]].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else 
            {
                var obj = Instantiate(inventory.Container[i].item.gameobject, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.gameobject, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            if (inventory.Container[i].item.type == ItemType.Book)
            {
                BookSO book = (BookSO)inventory.Container[i].item;
                obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = book.title;
                //Debug.Log(book.title);
            }
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS *(i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
