using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    [Header("消失設定")]
    public float blinkDuration = 2f;        // 閃爍階段的持續時間（秒）
    public float blinkInterval = 0.2f;      // 閃爍的間隔時間（秒）
    public float disappearDelay = 1f;       // 閃爍後消失的延遲時間（秒）

    private Collider2D tilemapCollider;
    private Renderer tilemapRenderer;
    private bool isBlinking = false;         // 是否正在閃爍

    void Start()
    {
        // 獲取該物件的 Collider 和 Renderer 元件
        tilemapCollider = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<Renderer>();

        if (tilemapCollider == null)
        {
            Debug.LogError("未找到 Collider2D 組件！請確保已添加 Collider2D。");
        }

        if (tilemapRenderer == null)
        {
            Debug.LogError("未找到 Renderer 組件！請確保已添加 Renderer。");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查碰撞的物件是否為玩家（根據標籤）
        if (collision.gameObject.CompareTag("Player") && !isBlinking)
        {
            // 開始協程，執行閃爍並消失的流程
            StartCoroutine(BlinkAndDisappear());
        }
    }

    /// <summary>
    /// 閃爍並在閃爍結束後消失的協程
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkAndDisappear()
    {
        isBlinking = true;

        float elapsed = 0f;

        // 閃爍階段：僅切換渲染器的可見性
        while (elapsed < blinkDuration)
        {
            ToggleRendererVisibility();
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // 確保方塊最終是不可見的
        SetRendererVisibility(false);

        // 延遲後完全消失（可選）
        yield return new WaitForSeconds(disappearDelay);

        // 停用碰撞器和渲染器
        UnVisualize();

        // 如果需要，可以在此處銷毀物件
        // Destroy(gameObject);

        isBlinking = false;
    }

    /// <summary>
    /// 切換渲染器的可見性，保持碰撞器啟用
    /// </summary>
    private void ToggleRendererVisibility()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = !tilemapRenderer.enabled;
        }
    }

    /// <summary>
    /// 設定渲染器的可見性，保持碰撞器啟用
    /// </summary>
    /// <param name="visible">是否可見</param>
    private void SetRendererVisibility(bool visible)
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = visible;
        }
    }

    /// <summary>
    /// 隱藏方塊並停用碰撞器和渲染器
    /// </summary>
    private void UnVisualize()
    {
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = false; // 停用碰撞器
        }
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false; // 停用渲染器
        }
    }

    /// <summary>
    /// 重置方塊，使其重新可見並可再次觸發閃爍
    /// </summary>
    public void ResetBlock()
    {
        SetRendererVisibility(true);
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true; // 確保碰撞器啟用
        }
        isBlinking = false;
    }
}
