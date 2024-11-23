using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public GameObject endMenuUI; // Reference to the end menu UI
     void Start()
    {
        endMenuUI.SetActive(false);
    }
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
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        endMenuUI.SetActive(true); // Show the end menu UI
        Time.timeScale = 0f; // Pause the game
    }

    public void LoadLevelSelector()
    {
        Debug.Log("Loading Level Selector...");
        Time.timeScale = 1f; // Reset game speed before loading the menu
        SceneManager.LoadScene("Level Selector"); // Load the main menu scene
    }
}
