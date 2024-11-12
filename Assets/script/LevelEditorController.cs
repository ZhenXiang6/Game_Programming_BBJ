using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelEditorController : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("選擇當前關卡")]
    public string selectedLevel;  // 選擇的關卡名稱

    [Tooltip("可用的關卡列表")]
    public List<string> availableLevels = new List<string> { "Level1", "Level2", "Level3" }; // 預定義的關卡列表

    public static MapData currentMapData;

    public GameObject[] blockPrefabs;
    public Transform mapContainer;
    public Button startGameButton;
    public Button clearButton;
    public Button saveMapButton;
    public Button selectModeButton;
    public GameObject darkOverlay;          // 暗色遮罩
    public int rotationAngle = 90;
    public float gridSpacing = 1f;

    private bool isSelectMode = false;             // 是否進入選取模式
    
    private GameObject previewBlock;
    private GameObject selectedBlockPrefab;
    private GameObject selectedBlock;
    private List<GameObject> placedPlayerBlocks = new List<GameObject>(); // 僅玩家方塊

    void Start()
    {
        
        LoadMapFromPlayerPrefs();

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

        if (hit.collider != null && placedPlayerBlocks.Contains(hit.collider.gameObject))
        {
            DeselectBlock(); // 取消之前的選中
            selectedBlock = hit.collider.gameObject;
            HighlightBlock(selectedBlock, true);
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
    if (EventSystem.current.IsPointerOverGameObject()) 
        return;
    if (selectedBlock != null)
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 targetPosition = SnapToGrid(mousePosition);

        if (Input.GetMouseButton(0)) // 按住左鍵拖動方塊
        {
            // 使用 OverlapCircle 檢查目標位置是否已有方塊
            float checkRadius = gridSpacing / 2f * 0.9f;
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
            placedPlayerBlocks.Remove(selectedBlock);
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

            color.a = highlight ? 0.7f : 1f;

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
        position.z = 0;
        
        // 使用 OverlapCircle 檢查位置附近是否已有方塊
        float checkRadius = gridSpacing / 2f * 0.9f;
        Collider2D overlap = Physics2D.OverlapCircle(position, checkRadius);

        if (overlap == null)
        {
            GameObject newBlock = Instantiate(selectedBlockPrefab, position, Quaternion.identity, mapContainer);
            placedPlayerBlocks.Add(newBlock);
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
    // 確保索引在範圍內，且未處於選取模式
    if (index >= 0 && index < blockPrefabs.Length && !isSelectMode)
    {
        // 清除當前的預覽方塊
        if (previewBlock != null)
        {
            Destroy(previewBlock);
        }

        // 選擇新的方塊並生成預覽
        selectedBlockPrefab = blockPrefabs[index];
        previewBlock = Instantiate(selectedBlockPrefab, mapContainer); // 將 mapContainer 設為父物件
        previewBlock.GetComponent<Collider2D>().enabled = false; // 關閉碰撞

        // 設定預覽方塊位置到鼠標位置
        Vector3 mousePosition = GetMouseWorldPosition();
        previewBlock.transform.position = SnapToGrid(mousePosition);

        // 清除已選中的已放置方塊
        selectedBlock = null;

        // 自動退出選取模式並更新按鈕狀態
        isSelectMode = false;
        selectModeButton.interactable = true;
    }
}




    // 儲存、載入、清除等其他功能
    public void SaveMap()
    {
        string MapKey = selectedLevel;
        MapData mapData = new MapData();

        foreach (GameObject block in placedPlayerBlocks)
        {
            BlockData blockData = new BlockData
            {
                blockType = block.name.Replace("(Clone)", ""),
                position = block.transform.position,
                rotation = block.transform.rotation.eulerAngles.z,
            };
            mapData.playerBlocks.Add(blockData);
        }

        currentMapData = mapData;
        string json = JsonUtility.ToJson(mapData, true);
        PlayerPrefs.SetString(MapKey, json);
        PlayerPrefs.Save();
        Debug.Log("Map saved to PlayerPrefs with key: " + MapKey);
    }

    public void LoadMapFromPlayerPrefs()
    {
        string MapKey = selectedLevel;
        if (PlayerPrefs.HasKey(MapKey))
        {
            string json = PlayerPrefs.GetString(MapKey, null);
            currentMapData = JsonUtility.FromJson<MapData>(json);
            LoadMap(currentMapData);
            Debug.Log("Map loaded from PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No saved map data found in PlayerPrefs.");
        }
    }


    void LoadMap(MapData mapData)
    {
        foreach (BlockData blockData in mapData.playerBlocks)
        {
            GameObject prefab = FindBlockPrefab(blockData.blockType);
            if (prefab != null)
            {
                GameObject newBlock = Instantiate(prefab, blockData.position, Quaternion.Euler(0, 0, blockData.rotation), mapContainer);
                placedPlayerBlocks.Add(newBlock);
            }
        }
    }

    private GameObject FindBlockPrefab(string blockType)
    {
        // 根據 blockType 找到對應的預置物
        // 假設 blockPrefabs 是所有方塊預置物的數組
        foreach (GameObject prefab in blockPrefabs)
        {
            if (prefab.name == blockType)
            {
                return prefab;
            }
        }
        return null;
    }

    private void ClearMap()
    {
        // 清空當前地圖中的所有方塊
        foreach (GameObject block in placedPlayerBlocks)
        {
            Destroy(block);
        }
        placedPlayerBlocks.Clear();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(selectedLevel);
    }
    private void OnApplicationQuit()
    {
        ClearMap();
        SaveMap();
    }
}

