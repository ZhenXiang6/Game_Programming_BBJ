using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenuTrigger : MonoBehaviour
{
    public GameObject endMenuUI; // Reference to the end menu UI

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        endMenuUI.SetActive(true); // Show the end menu UI
        Time.timeScale = 0f; // Pause the game
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1f; // Reset game speed before loading the menu
        SceneManager.LoadScene("Main"); // Load the main menu scene
    }
}
