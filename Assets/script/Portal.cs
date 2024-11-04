using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("目標場景的名稱")]
    public string targetSceneName; // 設置要切換的場景名稱
    public GameObject pressEText;

    private bool playerInRange = false; // 用來檢查玩家是否在觸發區域內

    private void Start()
    {
        pressEText.SetActive(false); // 一開始隱藏提示文字
    }

    private void Update()
    {
        // 當玩家在觸發區域內並按下 "E" 鍵時，切換到目標場景
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

    // 當玩家進入傳送門觸發區域時觸發
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 檢查進入的物件是否為玩家
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressEText.SetActive(true); // 顯示提示文字
        }
    }

    // 當玩家離開傳送門觸發區域時觸發
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEText.SetActive(false); // 隱藏提示文字
        }
    }
}
