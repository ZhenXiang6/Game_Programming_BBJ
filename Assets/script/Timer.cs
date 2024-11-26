using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text text; // 使用 TMP_Text 來支援 TextMeshPro
    private float timer;  // 用於追蹤經過的時間

    void Start()
    {
        timer = 0f; // 初始化計時器
        if (text != null)
        {
            text.text = "00:00"; // 初始化文字為 00:00
        }
    }

    void Update()
    {
        timer += Time.deltaTime; // 每幀增加經過的時間

        // 計算分鐘和秒數
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        // 更新文字顯示
        if (text != null)
        {
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
