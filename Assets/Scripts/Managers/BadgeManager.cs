using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgeManager : MonoBehaviour
{
    public static BadgeManager instance;
    public InventoryObject inventory;
    [Header("Badge List UI")]
    public GameObject[] badges;
    private TextMeshProUGUI[] badgeText;
    [Header("Badge View UI")]
    public GameObject badgeViewPanel;
    public TextMeshProUGUI badgeDescription;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    private Badge badgeSO;
    public bool badgeViewIsActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one  BadgeManager in the scene");
        }
        instance = this;
    }
    public static BadgeManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        badgeText = new TextMeshProUGUI[badges.Length];
        int index = 0;
        foreach (GameObject badge in badges)
        {
            badgeText[index] = badge.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        DisplayBadges();
        badgeViewIsActive = false;
        badgeViewPanel.SetActive(false);
    }
    void Update()
    {
        UpdateDisplay();
        if (badgeViewIsActive && Input.GetKeyDown(KeyCode.Escape))
        {
            badgeViewIsActive = false;
            badgeViewPanel.SetActive(false);
        }
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.BadgeContainer.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.BadgeContainer[i]))
            {
                if (inventory.BadgeContainer[i].item.type == ItemType.Badge)
                {
                    Badge badgeSO = (Badge)inventory.BadgeContainer[i].item;
                    badgeText[i].text = badgeSO.badgeDescription;
                    badges[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (inventory.BadgeContainer[i].item.type == ItemType.Badge)
                {
                    Badge badgeSO = (Badge)inventory.BadgeContainer[i].item;
                    badgeText[i].text = badgeSO.badgeDescription;
                    itemsDisplayed.Add(inventory.BadgeContainer[i], badgeSO.gameobject);
                    badges[i].gameObject.SetActive(true);
                }
            }
        }
    }
    private void DisplayBadges()
    {
        int index = 0;
        for (int i = index; i < inventory.BadgeContainer.Count; i++)
        {
            Badge badgeSO = (Badge)inventory.BadgeContainer[i].item;
            badgeText[i].text = badgeSO.badgeName;
            badges[i].gameObject.SetActive(true);
            itemsDisplayed.Add(inventory.BadgeContainer[i], badgeSO.gameobject);
            index++;
        }
        Debug.Log(index);

        //badges[0].gameObject.GetComponent<Button>().Select();
        //set ramaining UI choices inactive
        for (int i = index; i < badges.Length; i++)
        {
            badges[i].gameObject.SetActive(false);
        }
    }
    public void CheckButtonText(GameObject buttonObj)
    {
        // Get the Text component from the button and retrieve the text
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        string currentText = buttonText.text;

        for (int i = 0; i < inventory.BadgeContainer.Count; i++)
        {
            // Check if the current text matches any item in the name list
            Badge badgeSO = (Badge)inventory.BadgeContainer[i].item;
            if (badgeSO.badgeName == currentText)
            {
                badgeDescription.text = badgeSO.badgeDescription;
                badgeViewPanel.SetActive(true);
                badgeViewIsActive = true;
                //Debug.Log("Button text matches a name in the list: " + currentText);
            }
        }
    }

    
}
