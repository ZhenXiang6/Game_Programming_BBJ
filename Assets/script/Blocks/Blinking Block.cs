using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps; // 用於處理 Tilemaps

public class BlinkingBlock : MonoBehaviour
{
    [Header("階段持續時間設定（秒）")]
    public float existDuration = 5f;        // 存在階段的持續時間
    public float blinkDuration = 2f;        // 閃爍階段的持續時間
    public float disappearDuration = 3f;    // 消失階段的持續時間

    [Header("閃爍設定")]
    public float blinkInterval = 0.2f;      // 閃爍的間隔時間（秒）

    private bool isVisible = true;           // 當前方塊的可見狀態

    private Tilemap tilemap;                 // Tilemap 組件的引用
    private TilemapCollider2D tilemapCollider; // TilemapCollider2D 組件的引用

    void Start()
    {
        // 獲取 Tilemap 和 TilemapCollider2D 組件
        tilemap = GetComponent<Tilemap>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        // 檢查組件是否存在
        if (tilemap == null)
        {
            Debug.LogError("未找到 Tilemap 組件！請將 Tilemap 組件附加到該 GameObject。");
        }

        if (tilemapCollider == null)
        {
            Debug.LogError("未找到 TilemapCollider2D 組件！請將 TilemapCollider2D 組件附加到該 GameObject。");
        }

        // 啟動循環協程
        StartCoroutine(CycleBlinkDisappear());
    }

    /// <summary>
    /// 循環執行 存在 → 閃爍 → 消失 的階段
    /// </summary>
    /// <returns></returns>
    IEnumerator CycleBlinkDisappear()
    {
        while (true)
        {
            // 存在階段
            SetVisible(true);
            tilemapCollider.enabled = true;
            yield return new WaitForSeconds(existDuration);

            // 閃爍階段
            float blinkElapsed = 0f;
            while (blinkElapsed < blinkDuration)
            {
                ToggleVisibility();
                yield return new WaitForSeconds(blinkInterval);
                blinkElapsed += blinkInterval;
            }

            // 確保方塊在閃爍後是不可見的
            SetVisible(false);
            tilemapCollider.enabled = false;

            // 消失階段
            yield return new WaitForSeconds(disappearDuration);
        }
    }

    /// <summary>
    /// 切換方塊的可見性
    /// </summary>
    void ToggleVisibility()
    {
        isVisible = !isVisible;
        UpdateVisibility();
    }

    /// <summary>
    /// 設定方塊的可見性
    /// </summary>
    /// <param name="visible">是否可見</param>
    void SetVisible(bool visible)
    {
        isVisible = visible;
        UpdateVisibility();
    }

    /// <summary>
    /// 根據 isVisible 變數更新方塊的可見性和碰撞
    /// </summary>
    void UpdateVisibility()
    {
        if (tilemap != null)
        {
            // 顯示或隱藏 Tilemap
            tilemap.color = isVisible ? Color.white : new Color(1f, 1f, 1f, 0f); // 完全透明表示不可見
        }

        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = isVisible; // 啟用或禁用碰撞體
        }
    }

    /// <summary>
    /// 可選：立即停止循環並使方塊保持可見
    /// </summary>
    public void StopCycle()
    {
        StopAllCoroutines();
        SetVisible(true);
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true;
        }
    }
}
