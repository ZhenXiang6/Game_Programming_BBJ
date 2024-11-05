using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;          // 滾輪縮放速度
    public float minZoom = 5f;            // 最小縮放值
    public float maxZoom = 20f;           // 最大縮放值
    public float panSpeed = 0.5f;         // 拖動速度
    public Vector2 mapBounds = new Vector2(50f, 50f); // 地圖邊界大小

    private Vector3 dragOrigin;           // 拖動起始位置

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    // 滾輪縮放功能
    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0f)
        {
            float newSize = Camera.main.orthographicSize - scrollData * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }

    // 按住 ALT 並拖動畫面
    void HandlePan()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = Camera.main.transform.position + difference * panSpeed;

                // 限制攝影機位置在地圖邊界內
                newPosition.x = Mathf.Clamp(newPosition.x, -mapBounds.x, mapBounds.x);
                newPosition.y = Mathf.Clamp(newPosition.y, -mapBounds.y, mapBounds.y);

                Camera.main.transform.position = newPosition;
            }
        }
    }
}
