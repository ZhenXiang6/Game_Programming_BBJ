using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public GameObject endMenuUI; // Reference to the end menu UI
    public TMP_Text text;
    public TMP_Text getStars;
    public LevelData leveldata;
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

    int ComputeStars()
    {
        // 從文字中解析分鐘與秒數
        string[] timeParts = text.text.Split(':'); // 假設 `text` 是 "MM:SS" 格式
        int minute = int.Parse(timeParts[0]);
        int second = int.Parse(timeParts[1]);

        // 計算總秒數
        int totalSeconds = minute * 60 + second;

        // 比較條件，根據 LevelData 獲取的秒數設定星星
        if (totalSeconds <= leveldata.get_three_stars)
        {
            return 3;
        }
        else if (totalSeconds <= leveldata.get_two_stars)
        {
            return 2;
        }
        else if (totalSeconds <= leveldata.get_one_star)
        {
            return 1;
        }

        // 如果未達成條件，返回 0 星
        return 0;
    }

    void EndGame()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        endMenuUI.SetActive(true); // Show the end menu UI
        Time.timeScale = 0f; // Pause the game

        leveldata.earnedStars = ComputeStars(); // 更新星星數量
        
        getStars.text = $"You get {leveldata.earnedStars} star(s)!";
    }


    public void LoadLevelSelector()
    {
        Debug.Log("Loading Level Selector...");
        Time.timeScale = 1f; // Reset game speed before loading the menu
        SceneManager.LoadScene("Level Selector"); // Load the main menu scene
    }
}
