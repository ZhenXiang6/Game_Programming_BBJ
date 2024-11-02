using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public LevelData ld;
    public Image levelImage;       // The main image for Level1
    public Image[] stars;          // Array to hold the 3 star images
    public int starsEarned = 0;    // Number of stars earned, from 0 to 3
    public ArrowData arrow_data;
    public Button levelButton;

    void Start()
    {
        starsEarned = ld.earnedStars; // Assuming this retrieves the number of stars from LevelData
        Debug.Log("Starting with starsEarned: " + starsEarned);
        UpdateStars(starsEarned);

        // Add listener to button click
        levelButton.onClick.AddListener(OnLevelButtonClick);
    }

    // Method to update stars based on earned stars
    public void UpdateStars(int earnedStars)
    {
        starsEarned = Mathf.Clamp(earnedStars, 0, stars.Length); // Limit to 0-3 stars
        Debug.Log("Updating starsEarned to: " + starsEarned);

        for (int i = 0; i < stars.Length; i++)
        {
            bool isActive = i < starsEarned;
            stars[i].gameObject.SetActive(isActive);
            Debug.Log("Setting star " + (i + 1) + " active: " + isActive);
        }
    }

    // Method to handle the button click
    private void OnLevelButtonClick()
    {
        arrow_data.destination = ld.portalPosition;
        arrow_data.showArrow = true;
        Debug.Log("Arrow activated!");
    }
}
