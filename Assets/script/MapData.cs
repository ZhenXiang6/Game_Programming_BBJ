using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData
{
    public string blockType;       // 方塊類型
    public Vector3 position;       // 方塊位置
    public float rotation;         // 方塊旋轉角度

}

[System.Serializable]
public class MapData
{
    public List<BlockData> playerBlocks = new List<BlockData>();   // 玩家方塊
}
