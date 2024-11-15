using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public static BookManager instance;
    public InventoryObject inventory;
    [Header("Books List UI")]
    public GameObject[] books;
    private TextMeshProUGUI[] bookText;
    [Header("Book View UI")]
    public GameObject bookViewPanel;
    public TextMeshProUGUI bookContentUI;
    public TextMeshProUGUI bookPageUI;
    public Button prevBtn;
    public Button nextBtn;
    private List<string> pages = new List<string>();  // Holds paginated text content
    private int currentPage = 1;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    private BookSO bookSO;

    public bool bookViewIsActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one menu manager in the scene");
        }
        instance = this;
    }
    public static BookManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        bookText = new TextMeshProUGUI[books.Length];
        int index = 0;
        foreach (GameObject book in books)
        {
            bookText[index] = book.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        DisplayBooks();
        bookViewIsActive = false;
        bookViewPanel.SetActive(false);
    }
    void Update()
    {
        UpdateDisplay();
        if (bookViewIsActive && Input.GetKeyDown(KeyCode.Escape))
        {
            bookViewIsActive = false;
            bookViewPanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                if (inventory.Container[i].item.type == ItemType.Book)
                {
                    BookSO book = (BookSO)inventory.Container[i].item;
                    bookText[i].text = book.title;
                    books[i].gameObject.SetActive(true);
                }            }
            else
            {
                if (inventory.Container[i].item.type == ItemType.Book)
                {
                    BookSO book = (BookSO)inventory.Container[i].item;
                    bookText[i].text = book.title;
                    itemsDisplayed.Add(inventory.Container[i], book.gameobject);
                    books[i].gameObject.SetActive(true);
                }
            }
        }
    }
    private void DisplayBooks()
    {
        int index = 0;
        for (int i = index; i < inventory.Container.Count; i++)
        {
            BookSO book = (BookSO)inventory.Container[i].item;
            bookText[i].text = book.title;
            books[i].gameObject.SetActive(true);
            itemsDisplayed.Add(inventory.Container[i], book.gameobject);
            Debug.Log(book.title);
            index++;
        }
        Debug.Log(index);

        //books[0].gameObject.GetComponent<Button>().Select();
        //set ramaining UI choices inactive
        for (int i = index; i < books.Length; i++) 
        {
            books[i].gameObject.SetActive(false);
        }
    }
    public void CheckButtonText(GameObject buttonObj)
    {
        // Get the Text component from the button and retrieve the text
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        string currentText = buttonText.text;

        for (int i = 0; i < inventory.Container.Count; i++)
        {
            // Check if the current text matches any item in the name list
            BookSO bookSO = (BookSO)inventory.Container[i].item;
            if (bookSO.title == currentText)
            {
                bookContentUI.text = bookSO.content;
                bookViewPanel.SetActive(true);
                bookViewIsActive = true;
                //Debug.Log("Button text matches a name in the list: " + currentText);
            }
        }
    }

    private void UpdatePagination()
    {
        bookPageUI.text = bookContentUI.pageToDisplay.ToString();
    }

    public void NextPage()
    {
        int totalPages = bookContentUI.textInfo.pageCount;
        //Debug.Log(totalPages);
        if (currentPage >= totalPages) //check if book current page will exceed book page
            return;
        if (currentPage < totalPages)
        {
            currentPage++;
            bookContentUI.pageToDisplay += 1;
            Debug.Log("current page is " + currentPage);
            UpdatePagination();
        }
    }

    public void PreviousPage()
    {
        if (currentPage <= 1)
            return;
        if (currentPage > 1)
        {
            currentPage--;
            bookContentUI.pageToDisplay--;
            UpdatePagination();
        }
    }
}
