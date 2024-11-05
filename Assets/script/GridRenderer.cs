using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public float gridSpacing = 1f;       // 網格間距
    public int gridSize = 100;           // 網格大小（範圍）
    public Color gridColor = Color.gray; // 網格顏色

    private void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        // 生成容器物件來存放網格線
        GameObject gridContainer = new GameObject("GridContainer");

        // 水平線
        for (float x = -gridSize; x <= gridSize; x += gridSpacing)
        {
            GameObject line = new GameObject("GridLine_H_" + x);
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x, -gridSize, 0));
            lr.SetPosition(1, new Vector3(x, gridSize, 0));
            lr.startWidth = 0.05f;  // 線條寬度
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            line.transform.parent = gridContainer.transform;
        }

        // 垂直線
        for (float y = -gridSize; y <= gridSize; y += gridSpacing)
        {
            GameObject line = new GameObject("GridLine_V_" + y);
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(-gridSize, y, 0));
            lr.SetPosition(1, new Vector3(gridSize, y, 0));
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            line.transform.parent = gridContainer.transform;
        }
    }
}
