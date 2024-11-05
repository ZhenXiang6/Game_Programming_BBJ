using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData
{
    public string blockType;     // 方塊的類型
    public Vector3 position;     // 方塊的位置
    public float rotation;       // 方塊的旋轉角度 (Z 軸)
}

[System.Serializable]
public class MapData
{
    public List<BlockData> blocks = new List<BlockData>();
}