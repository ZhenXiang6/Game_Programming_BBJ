using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject[] blockPrefabs; // 各類型方塊的預置物
    public Transform mapContainer;     // 存放方塊的容器

    void Start()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        string path = Application.persistentDataPath + "/mapData_level1.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MapData mapData = JsonUtility.FromJson<MapData>(json);

            foreach (BlockData blockData in mapData.blocks)
            {
                GameObject prefab = FindBlockPrefab(blockData.blockType);
                if (prefab != null)
                {
                    Instantiate(prefab, blockData.position, Quaternion.Euler(0, 0, blockData.rotation), mapContainer);
                }
            }
            Debug.Log("Map loaded from " + path);
        }
        else
        {
            Debug.LogWarning("No saved map data found at " + path);
        }
    }

    private GameObject FindBlockPrefab(string blockType)
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
