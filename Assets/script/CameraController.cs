using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;          // 滾輪縮放速度
    public float minZoom = 5f;            // 最小縮放值
    public float maxZoom = 20f;           // 最大縮放值
    public float panSpeed = 0.5f;         // 拖動速度
    public GameObject Background;
    
    private Vector2 mapBounds;            // 地圖邊界大小
    private Camera mainCamera;
    private Vector3 dragOrigin;           // 拖動起始位置

    private void Start()
    {
        mainCamera = Camera.main;

        // 確保 Background 存在並計算其邊界
        Renderer backgroundRenderer = Background.GetComponent<Renderer>();
        if (backgroundRenderer != null)
        {
            // 計算背景的半寬和半高，作為邊界的最大範圍
            mapBounds = new Vector2(
                backgroundRenderer.bounds.size.x / 2,
                backgroundRenderer.bounds.size.y / 2
            );

            // 將攝影機定位到背景的中心
            PositionCameraToCenter();
        }
        else
        {
            Debug.LogError("Background GameObject 缺少 Renderer 組件");
        }
    }

    private void PositionCameraToCenter()
    {
        // 計算背景的中心點
        Vector3 backgroundCenter = Background.transform.position;

        // 計算攝影機的視野範圍
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // 確保攝影機的視野完全位於背景內
        float xPosition = Mathf.Clamp(backgroundCenter.x, -mapBounds.x + cameraWidth, mapBounds.x - cameraWidth);
        float yPosition = Mathf.Clamp(backgroundCenter.y, -mapBounds.y + cameraHeight, mapBounds.y - cameraHeight);

        // 設定攝影機位置
        mainCamera.transform.position = new Vector3(xPosition, yPosition, mainCamera.transform.position.z);
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0f)
        {
            float newSize = mainCamera.orthographicSize - scrollData * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            ConstrainCameraWithinBounds(); // 縮放後檢查邊界
        }
    }

    void HandlePan()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 設置拖動的起始位置
                dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = mainCamera.transform.position + difference * panSpeed;

                mainCamera.transform.position = newPosition;
                ConstrainCameraWithinBounds(); // 拖動後檢查邊界
            }
        }
    }

    void ConstrainCameraWithinBounds()
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 position = mainCamera.transform.position;

        float minX = -mapBounds.x + cameraWidth;
        float maxX = mapBounds.x - cameraWidth;
        float minY = -mapBounds.y + cameraHeight;
        float maxY = mapBounds.y - cameraHeight;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        mainCamera.transform.position = position;
    }
}
