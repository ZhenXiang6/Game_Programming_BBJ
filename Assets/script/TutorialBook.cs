using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialBook : MonoBehaviour
{
    public List<GameObject> pages; // List of page objects
    public TextMeshProUGUI pageText; // UI Text to display the current page
    private int current_page = 0; // Tracks the current page

    // Go to the next page
    public void NextPage()
    {
        // Hide the current page
        pages[current_page].SetActive(false);

        current_page++;
        current_page %= pages.Count;
        // Move to the next page
        
        pages[current_page].SetActive(true);

        UpdatePageText();
    }

    // Go to the previous page
    public void PrevPage()
    {
        // Hide the current page
        pages[current_page].SetActive(false);

        // Move to the previous page
        current_page--;
        
        if (current_page < 0) {
            current_page = pages.Count-1;
        }

        pages[current_page].SetActive(true);

        UpdatePageText();
    }

    // Update the page text to show the current page
    private void UpdatePageText()
    {
        if (pageText != null)
        {
            pageText.text = $"{current_page + 1} / {pages.Count}";
        }
        else
        {
            Debug.LogWarning("Page Text UI is not assigned!");
        }
    }

    // Initialize the book (optional, hides all but the first page)
    void Start()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == 0); // Show only the first page
        }

        UpdatePageText();
    }
}
