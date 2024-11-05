using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEditorController : MonoBehaviour
{
    public static MapData currentMapData;

    public GameObject[] blockPrefabs;      // 方塊預置物的數組
    public Transform mapContainer;         // 地圖方塊的父物件
    public Button startGameButton;         // 開始遊戲按鈕
    public Button clearButton;             // 清除地圖按鈕
    public Button saveMapButton;           // 儲存地圖按鈕
    public Button loadMapButton;           // 載入地圖按鈕
    public Button resetMapButton;          // 重置地圖按鈕
    public Toggle deleteModeToggle;        // 刪除模式按鈕
    public GameObject selectedBlockPrefab; // 當前選擇的方塊
    public int rotationAngle = 90;         // 每次旋轉的角度
    private bool isDeleteMode = false;     // 刪除模式開關

    private List<GameObject> placedBlocks = new List<GameObject>();
    private float gridSpacing;

    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        clearButton.onClick.AddListener(ClearMap);
        saveMapButton.onClick.AddListener(SaveMap);
        loadMapButton.onClick.AddListener(LoadMap);
        resetMapButton.onClick.AddListener(ResetMap);
        deleteModeToggle.onValueChanged.AddListener(ToggleDeleteMode);

        if (currentMapData == null)
        {
            LoadDefaultMap();
        }
        else
        {
            LoadMap(currentMapData);
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isDeleteMode)
            {
                DeleteBlock();
            }
            else
            {
                PlaceBlock();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelectedBlock();
        }
    }

    void PlaceBlock()
    {
        if (selectedBlockPrefab != null)
        {
        Vector3 mousePosition = GetMouseWorldPosition();
        
        // 將鼠標位置對齊至最近的網格點
        mousePosition.x = Mathf.Round(mousePosition.x / gridSpacing) * gridSpacing;
        mousePosition.y = Mathf.Round(mousePosition.y / gridSpacing) * gridSpacing;
        
        GameObject newBlock = Instantiate(selectedBlockPrefab, mousePosition, Quaternion.identity, mapContainer);
        placedBlocks.Add(newBlock);
        }
    }


    void DeleteBlock()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && placedBlocks.Contains(hit.collider.gameObject))
        {
            placedBlocks.Remove(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SelectBlock(int index)
    {
        if (index >= 0 && index < blockPrefabs.Length)
        {
            selectedBlockPrefab = blockPrefabs[index];
            isDeleteMode = false; // 取消刪除模式
        }
    }

    public void ToggleDeleteMode(bool isOn)
    {
        isDeleteMode = isOn;
        if (isDeleteMode)
        {
            Debug.Log("DeleteModeOn");
            selectedBlockPrefab = null; // 在刪除模式下清除選中的方塊
        }
    }
    public void RotateSelectedBlock()
    {
        if (selectedBlockPrefab != null)
        {
            selectedBlockPrefab.transform.Rotate(0, 0, rotationAngle);
        }
    }

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
        File.WriteAllText(Application.persistentDataPath + "/mapData.json", json);
        Debug.Log("Map saved to " + Application.persistentDataPath + "/mapData.json");
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
        string path = Application.persistentDataPath + "/mapData.json";
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
        placedBlocks.Add(defaultBlock);
    }

    void StartGame()
    {
        SceneManager.LoadScene("RunScene");
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

