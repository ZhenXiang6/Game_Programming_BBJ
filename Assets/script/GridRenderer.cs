using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public float gridSpacing = 1f;        // 網格間距
    public Color gridColor = Color.gray;  // 網格顏色
    public GameObject Background;         // 背景物件

    private Vector2 mapBounds;            // 地圖邊界大小
    private Vector3 backgroundCenter;     // 背景中心位置

    private void Start()
    {
        // 確保 Background 存在並計算其邊界
        Renderer backgroundRenderer = Background.GetComponent<Renderer>();
        if (backgroundRenderer != null)
        {
            // 計算背景的半寬和半高作為邊界
            mapBounds = new Vector2(
                backgroundRenderer.bounds.size.x / 2,
                backgroundRenderer.bounds.size.y / 2
            );

            // 獲取背景的中心位置
            backgroundCenter = Background.transform.position;

            // 繪製網格
            DrawGrid();
        }
        else
        {
            Debug.LogError("Background GameObject 缺少 Renderer 組件");
        }
    }

    void DrawGrid()
    {
        // 生成容器物件來存放網格線
        GameObject gridContainer = new GameObject("GridContainer");

        // 從中心向左右擴展的水平線
        for (float x = 0; x <= mapBounds.x; x += gridSpacing)
        {
            // 向右擴展
            CreateLine(new Vector3(x + backgroundCenter.x, -mapBounds.y + backgroundCenter.y, 0),
                       new Vector3(x + backgroundCenter.x, mapBounds.y + backgroundCenter.y, 0),
                       gridContainer, "GridLine_H_Right_" + x);

            // 向左擴展
            if (x != 0) // 避免重複中心線
            {
                CreateLine(new Vector3(-x + backgroundCenter.x, -mapBounds.y + backgroundCenter.y, 0),
                           new Vector3(-x + backgroundCenter.x, mapBounds.y + backgroundCenter.y, 0),
                           gridContainer, "GridLine_H_Left_" + x);
            }
        }

        // 從中心向上下擴展的垂直線
        for (float y = 0; y <= mapBounds.y; y += gridSpacing)
        {
            // 向上擴展
            CreateLine(new Vector3(-mapBounds.x + backgroundCenter.x, y + backgroundCenter.y, 0),
                       new Vector3(mapBounds.x + backgroundCenter.x, y + backgroundCenter.y, 0),
                       gridContainer, "GridLine_V_Top_" + y);

            // 向下擴展
            if (y != 0) // 避免重複中心線
            {
                CreateLine(new Vector3(-mapBounds.x + backgroundCenter.x, -y + backgroundCenter.y, 0),
                           new Vector3(mapBounds.x + backgroundCenter.x, -y + backgroundCenter.y, 0),
                           gridContainer, "GridLine_V_Bottom_" + y);
            }
        }
    }

    // 建立網格線的輔助方法
    void CreateLine(Vector3 start, Vector3 end, GameObject parent, string name)
    {
        GameObject line = new GameObject(name);
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = gridColor;
        lr.endColor = gridColor;
        line.transform.parent = parent.transform;
    }
}
