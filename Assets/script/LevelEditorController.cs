using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelEditorController : MonoBehaviour
{
    public static MapData currentMapData;

    public GameObject[] blockPrefabs;
    public Transform mapContainer;
    public Button startGameButton;
    public Button clearButton;
    public Button saveMapButton;
    public Button loadMapButton;
    public Button resetMapButton;
    public Button selectModeButton;
    public GameObject darkOverlay;          // 暗色遮罩
    public int rotationAngle = 90;
    public float gridSpacing = 1f;

    private bool isSelectMode = false;             // 是否進入選取模式
    
    private GameObject previewBlock;
    private GameObject selectedBlockPrefab;
    private GameObject selectedBlock;
    private List<GameObject> placedBlocks = new List<GameObject>();
    private List<GameObject> initialBlocks = new List<GameObject>();

    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        clearButton.onClick.AddListener(ClearMap);
        saveMapButton.onClick.AddListener(SaveMap);
        loadMapButton.onClick.AddListener(LoadMap);
        resetMapButton.onClick.AddListener(ResetMap);
        selectModeButton.onClick.AddListener(ToggleSelectMode);

        if (currentMapData == null)
        {
            LoadDefaultMap();
        }
        else
        {
            LoadMap(currentMapData);
        }

        // 隱藏暗色遮罩
        darkOverlay.SetActive(false);
    }

    void Update()
    {
        if (isSelectMode)
        {
            HandleBlockSelection();
            HandleSelectedBlockMovement();
            HandleBlockDeletion();
        }
        else
        {
            HandlePreviewBlock();
            HandleBlockPlacement();
        }
    }

    // 切換選取模式
   public void ToggleSelectMode()
{
    isSelectMode = !isSelectMode;
    
    if (isSelectMode)
    {
        // 進入選取模式，顯示暗色遮罩
        darkOverlay.SetActive(true);
        selectedBlockPrefab = null;
        if (previewBlock != null)
        {
            Destroy(previewBlock); // 清除預覽方塊
        }
    }
    else
    {
        // 退出選取模式，隱藏暗色遮罩並取消選中
        darkOverlay.SetActive(false);
        DeselectBlock();
    }
}

    // 處理選取模式下的方塊選取
    void HandleBlockSelection()
{
    // 如果滑鼠在 UI 上，則不進行選取操作
    if (EventSystem.current.IsPointerOverGameObject()) 
        return;

    if (Input.GetMouseButtonDown(0))
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && placedBlocks.Contains(hit.collider.gameObject))
        {
            DeselectBlock(); // 取消之前的選中
            selectedBlock = hit.collider.gameObject;
            HighlightBlock(selectedBlock, true);
        }
        else
        {
            ToggleSelectMode(); // 點擊空白或非法方塊時，退出選取模式
        }
    }
}
    // 取消選中的方塊
    void DeselectBlock()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (selectedBlock != null)
        {
            HighlightBlock(selectedBlock, false); // 移除高亮效果
            selectedBlock = null;
        }
    }

    // 控制選中方塊的移動和旋轉
    void HandleSelectedBlockMovement()
{
    if (selectedBlock != null)
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 targetPosition = SnapToGrid(mousePosition);

        if (Input.GetMouseButton(0)) // 按住左鍵拖動方塊
        {
            // 使用 OverlapCircle 檢查目標位置是否已有方塊
            float checkRadius = gridSpacing / 2f;
            Collider2D overlap = Physics2D.OverlapCircle(targetPosition, checkRadius);

            // 如果重疊對象為空或者為當前選中方塊本身，則允許移動
            if (overlap == null || overlap.gameObject == selectedBlock)
            {
                selectedBlock.transform.position = targetPosition;
            }
            else
            {
                Debug.Log("該位置已有方塊，無法移動到該位置。");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Q 鍵逆時針旋轉
        {
            selectedBlock.transform.Rotate(0, 0, rotationAngle);
        }

        if (Input.GetKeyDown(KeyCode.E)) // E 鍵順時針旋轉
        {
            selectedBlock.transform.Rotate(0, 0, -rotationAngle);
        }
    }
}


    // 刪除選中的方塊
    void HandleBlockDeletion()
    {
        if (selectedBlock != null && Input.GetKeyDown(KeyCode.Delete))
        {
            placedBlocks.Remove(selectedBlock);
            Destroy(selectedBlock);
            selectedBlock = null;
        }
    }

    // 高亮或取消高亮顯示選中的方塊
    void HighlightBlock(GameObject block, bool highlight)
    {
        var spriteRenderer = block.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;

            color.a = highlight ? 0.5f : 1f;

            spriteRenderer.color = color;
        }
    }


    // 使預覽方塊跟隨鼠標移動
    void HandlePreviewBlock()
    {
        if (previewBlock != null && selectedBlockPrefab != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            previewBlock.transform.position = SnapToGrid(mousePosition);
        }
    }

    // 處理方塊放置
    void HandleBlockPlacement()
{
    // 如果滑鼠在 UI 上，則不進行放置方塊操作
    if (selectedBlockPrefab == null || isSelectMode || EventSystem.current.IsPointerOverGameObject()) 
        return;

    if (Input.GetMouseButtonDown(0) && previewBlock != null) // 左鍵放置新方塊
    {
        Vector3 position = previewBlock.transform.position;
        
        // 使用 OverlapCircle 檢查位置附近是否已有方塊
        float checkRadius = gridSpacing / 2f;
        Collider2D overlap = Physics2D.OverlapCircle(position, checkRadius);

        if (overlap == null)
        {
            GameObject newBlock = Instantiate(selectedBlockPrefab, position, Quaternion.identity, mapContainer);
            placedBlocks.Add(newBlock);
        }
        else
        {
            Debug.Log("該位置已有方塊，無法放置。");
        }
    }
}



    // 將位置對齊到網格
      Vector3 SnapToGrid(Vector3 position)
{
    // 計算網格對齊的中心點
    float snappedX = Mathf.Round((position.x - gridSpacing / 2f) / gridSpacing) * gridSpacing + gridSpacing / 2f;
    float snappedY = Mathf.Round((position.y - gridSpacing / 2f) / gridSpacing) * gridSpacing + gridSpacing / 2f;

    return new Vector3(snappedX, snappedY, position.z);
}


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    // 選擇一個方塊類型並顯示預覽
    public void SelectBlock(int index)
    {
        if (index >= 0 && index < blockPrefabs.Length)
        {
            if (previewBlock != null)
            {
                Destroy(previewBlock);
            }

            selectedBlockPrefab = blockPrefabs[index];
            previewBlock = Instantiate(selectedBlockPrefab);
            previewBlock.GetComponent<Collider2D>().enabled = false; // 關閉預覽方塊的碰撞
            selectedBlock = null; // 取消選中的已放置方塊
            isSelectMode = false; // 自動退出選取模式
            selectModeButton.interactable = true; // 更新選取模式按鈕狀態
        }
    }



    // 儲存、載入、清除等其他功能
    public void SaveMap()
    {
        MapData mapData = new MapData();

        foreach (GameObject block in placedBlocks)
        {
            BlockData blockData = new BlockData();
            blockData.blockType = block.name.Replace("(Clone)", "");
            blockData.position = block.transform.position;
            blockData.rotation = block.transform.rotation.eulerAngles.z;
            mapData.blocks.Add(blockData);
        }

        currentMapData = mapData;
        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(Application.persistentDataPath + "/mapData_level1.json", json);
        Debug.Log("Map saved to " + Application.persistentDataPath + "/mapData_level1.json");
    }

    public void LoadMap(MapData mapData)
    {
        ClearMap();

        foreach (BlockData blockData in mapData.blocks)
        {
            GameObject prefab = FindBlockPrefab(blockData.blockType);
            if (prefab != null)
            {
                GameObject newBlock = Instantiate(prefab, blockData.position, Quaternion.Euler(0, 0, blockData.rotation), mapContainer);
                placedBlocks.Add(newBlock);
            }
        }
    }

    public void LoadMap()
    {
        string path = Application.persistentDataPath + "/mapData_level1.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MapData mapData = JsonUtility.FromJson<MapData>(json);
            LoadMap(mapData);
            Debug.Log("Map loaded from " + path);
        }
        else
        {
            Debug.LogWarning("No saved map data found at " + path);
        }
    }

    public void ClearMap()
    {
        foreach (GameObject block in placedBlocks)
        {
            Destroy(block);
        }
        placedBlocks.Clear();
    }

    public void ResetMap()
    {
        currentMapData = null;
        ClearMap();
        LoadDefaultMap();
    }

    void LoadDefaultMap()
    {
        GameObject defaultBlock = Instantiate(blockPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity, mapContainer);
        initialBlocks.Add(defaultBlock);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    GameObject FindBlockPrefab(string blockType)
    {
        foreach (GameObject prefab in blockPrefabs)
        {
            if (prefab.name == blockType)
            {
                return prefab;
            }
        }
        return null;
    }
}
