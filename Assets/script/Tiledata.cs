using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileData", menuName = "LevelEditor/TileData")]
public class TileData : TileBase
{
    public Sprite tileSprite;
    public string blockType;
    public GameObject BlockPrefab;
}

