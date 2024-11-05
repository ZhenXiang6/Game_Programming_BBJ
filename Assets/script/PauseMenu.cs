using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public GameObject bagPanel; // Reference to the Bag panel
    public GameObject endMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (endMenuUI == null){
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
            else if(endMenuUI.activeSelf == false)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        bagPanel.SetActive(false); // Hide the Bag panel
        Time.timeScale = 1f; // Resume the game
        GameIsPaused = false; // Set the paused state to false
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        bagPanel.SetActive(false); // Ensure the Bag panel is inactive when pausing
        Time.timeScale = 0f; // Pause the game
        GameIsPaused = true; // Set the paused state to true
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu....");
        Resume(); // Resume the game before loading the menu
        SceneManager.LoadScene("Main"); // Load the main menu scene
    }
}
