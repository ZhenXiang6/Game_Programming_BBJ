using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    private Collider2D tilemapCollider;
    private Renderer tilemapRenderer;

    void Start()
    {
        // 獲取該物件的 Collider 和 Renderer 元件
        tilemapCollider = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<Renderer>();
    }

    void ResetBlocks()
    {
        Visualize();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 開始協程，延遲讓方塊消失
        StartCoroutine(DisappearAfterDelay(1f)); // 延遲 1 秒
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        // 等待指定的時間
        yield return new WaitForSeconds(delay);

        // 延遲結束後隱藏方塊
        UnVisualize();
    }

    void Visualize()
    {
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = true; // 啟用碰撞器
        }
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = true; // 啟用渲染器
        }
    }

    void UnVisualize()
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
}
