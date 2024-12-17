using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class BookLoader : MonoBehaviour
{
    [SerializeField] private List<BookSO> books;  // Assigned in the Inspector

    private void Start()
    {
        LoadBooksFromPlayFab();
    }

    private void LoadBooksFromPlayFab()
    {
        // Fetch the books data from PlayFab
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result.Data.TryGetValue("Books", out string booksJson))
            {
                try
                {
                    // Deserialize JSON into a list of BookData using Newtonsoft.Json
                    List<BookData> bookDataList = JsonConvert.DeserializeObject<List<BookData>>(booksJson);

                    // Iterate over the list of book data and update BookSO content
                    foreach (var bookData in bookDataList)
                    {
                        // Find the corresponding BookSO by title
                        foreach (var bookSO in books)
                        {
                            if (bookSO.title == bookData.title)
                            {
                                // Update the content of the BookSO
                                bookSO.content = bookData.content;
                                Debug.Log($"Updated {bookSO.title} with new content.");
                            }
                        }
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogError($"Error parsing JSON: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError("No 'Books' key found in PlayFab TitleData.");
            }
        },
        error =>
        {
            Debug.LogError("Failed to load books from PlayFab: " + error.GenerateErrorReport());
        });
    }
}

[System.Serializable]
public class BookData
{
    public string title;
    public string content;
}

//[System.Serializable]
//public class BookDataListWrapper
//{
//    public List<BookData> Books;
//}
