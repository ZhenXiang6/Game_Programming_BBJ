using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("方块预制体")]
    public GameObject blockPrefab;

    [Header("预览方块")]
    public GameObject previewBlockPrefab;
    private GameObject previewBlock;

    [Header("建造层")]
    public LayerMask buildLayer;

    [Header("建造限制")]
    public int maxBlocks = 100;
    private int currentBlockCount = 0;

    void Start()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("BlockBuilder: 未分配 Block Prefab！");
        }

        if (previewBlockPrefab != null)
        {
            previewBlock = Instantiate(previewBlockPrefab);
            // 使预览方块半透明
            SpriteRenderer sr = previewBlock.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 0.5f;
                sr.color = color;
            }
        }
    }

    void Update()
    {
        HandlePreview();
        HandleBuild();
    }

    void HandlePreview()
    {
        if (previewBlock == null)
            return;

        // 获取鼠标世界坐标
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 确保在同一平面

        // 可选：将位置对齐到网格
        Vector3 gridPos = new Vector3(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y),
            0f
        );

        previewBlock.transform.position = gridPos;
    }

    void HandleBuild()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentBlockCount >= maxBlocks)
            {
                Debug.Log("BlockBuilder: 已达到建造限制！");
                return;
            }

            // 获取鼠标世界坐标
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // 确保在同一平面

            // 可选：将位置对齐到网格
            Vector3 gridPos = new Vector3(
                Mathf.Round(mousePos.x),
                Mathf.Round(mousePos.y),
                0f
            );

            // 检查当前位置是否已有方块（避免重叠）
            Collider2D hit = Physics2D.OverlapBox(gridPos, Vector2.one, 0f, buildLayer);
            if (hit != null)
            {
                Debug.Log("BlockBuilder: 当前位置已有方块！");
                return;
            }

            // 实例化方块
            Instantiate(blockPrefab, gridPos, Quaternion.identity);
            currentBlockCount++;
            Debug.Log("BlockBuilder: 方块已建造！总数：" + currentBlockCount);
        }
    }

    // 可选：在 Scene 视图中可视化建造区域
    private void OnDrawGizmosSelected()
    {
        // 绘制一个覆盖区域用于检测已有方块
        Vector3 mousePos = Camera.main != null ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
        mousePos.z = 0f;
        Vector3 gridPos = new Vector3(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y),
            0f
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(gridPos, Vector3.one);
    }
}
