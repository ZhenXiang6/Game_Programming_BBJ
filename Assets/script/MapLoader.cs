using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private const string MapKey = "MapData_Level1"; // 與 LevelEditorController 中使用的鍵保持一致
    public GameObject[] blockPrefabs;              // 各種類型的方塊預置物
    public Transform mapContainer;                 // 地圖方塊的父容器

    void Start()
    {
        LoadMapFromPlayerPrefs();
    }

    // 從 PlayerPrefs 加載地圖並生成方塊
    public void LoadMapFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(MapKey))
        {
            // 從 PlayerPrefs 讀取 JSON 字符串並解析
            string json = PlayerPrefs.GetString(MapKey);
            MapData mapData = JsonUtility.FromJson<MapData>(json);

            // 根據地圖數據生成方塊
            foreach (BlockData blockData in mapData.defaultBlocks)
            {
                GameObject prefab = FindBlockPrefab(blockData.blockType);
                if (prefab != null)
                {
                    Instantiate(prefab, blockData.position, Quaternion.Euler(0, 0, blockData.rotation), mapContainer);
                }
            }
            foreach (BlockData blockData in mapData.playerBlocks)
            {
                GameObject prefab = FindBlockPrefab(blockData.blockType);
                if (prefab != null)
                {
                    Instantiate(prefab, blockData.position, Quaternion.Euler(0, 0, blockData.rotation), mapContainer);
                }
            }
            Debug.Log("Map loaded from PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No map data found in PlayerPrefs.");
        }
    }

    // 根據方塊類型找到對應的預置物
    private GameObject FindBlockPrefab(string blockType)
    {
        foreach (GameObject prefab in blockPrefabs)
        {
            if (prefab.name == blockType)
            {
                return prefab;
            }
        }
        Debug.LogWarning("Prefab not found for block type: " + blockType);
        return null;
    }
}
