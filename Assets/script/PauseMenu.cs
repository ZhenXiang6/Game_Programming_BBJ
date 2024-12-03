using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject confirmationPanel; // 確認視窗
    public string mainMenuSceneName = "MainMenu"; // 主選單場景名稱
    public LevelData levelData;

    void Update()
    {
        // 按下 Escape 鍵時切換暫停/繼續狀態
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果確認視窗顯示中，則隱藏確認視窗
            if (confirmationPanel.activeSelf)
            {
                CancelMainMenu();
            }
            else
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
        // 繼續遊戲
        pauseMenuUI.SetActive(false);
        confirmationPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        // 暫停遊戲
        pauseMenuUI.SetActive(true);
        confirmationPanel.SetActive(false); // 確認視窗默認為隱藏
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ReloadLevel()
    {
        Debug.Log("Reloading current level...");
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadEditorScene()
    {
        Debug.Log("Loading Editor Scene...");
        Resume();
        SceneManager.LoadScene(levelData.levelName + "_editor");
    }

    public void ShowConfirmationPanel()
    {
        // 顯示確認視窗，隱藏暫停菜單的其他部分
        confirmationPanel.SetActive(true);
    }

    public void ConfirmMainMenu()
    {
        // 確認返回主選單
        Debug.Log("Returning to Main Menu: " + mainMenuSceneName);
        Resume();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void CancelMainMenu()
    {
        // 取消返回主選單，隱藏確認視窗
        confirmationPanel.SetActive(false);
    }
}
