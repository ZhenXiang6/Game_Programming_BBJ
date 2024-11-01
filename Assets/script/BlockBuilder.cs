using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlockBuilder : MonoBehaviour
{
    [Header("放置方块参数")]
    public GameObject blackBlockPrefab; // 黑色方块预制体
    public GameObject redBlockPrefab;   // 红色方块预制体
    public Transform buildPoint;        // 方块放置的位置基准点
    public float placeRange = 0.5f;     // 放置范围
    public float verticalOffset = -1f;  // 角色脚下的垂直偏移量
    public float horizontalOffset = 1f; // 水平方向的偏移量，根据面向调整

    public string[] block_types = { "black", "red" };
    private string current_block_type;

    // Dictionary to store lists of placed blocks by color
    private Dictionary<string, List<GameObject>> placedBlocks = new Dictionary<string, List<GameObject>>()
    {
        { "black", new List<GameObject>() },
        { "red", new List<GameObject>() }
    };

    // Dictionary for tracking remaining blocks for each type
    private Dictionary<string, int> blocksHold = new Dictionary<string, int>()
    {
        { "black", 10 },
        { "red", 10 }
    };

    // Text for block counts
    public Text blocksNum;
    public Image block_color;
    public Sprite blackBlockSprite;
    public Sprite redBlockSprite;

    private SpriteRenderer spriteRenderer; // 用于检测角色面向方向

    void Start()
    {
        // 获取 SpriteRenderer 组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("BlockBuilder: SpriteRenderer 组件未找到，请确保 Player 对象上添加了 SpriteRenderer 组件。");
        }

        current_block_type = block_types[0]; // 初始化当前方块类型
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaceBlock();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            NextBlock();
        }

        UpdateUI();
    }

    void NextBlock()
    {
        int currentIndex = System.Array.IndexOf(block_types, current_block_type);
        currentIndex = (currentIndex + 1) % block_types.Length; // 循环到下一个方块类型
        current_block_type = block_types[currentIndex];
        Debug.Log($"Switched to {current_block_type} block.");
    }

    void PlaceBlock()
    {
        string color = current_block_type;
        if (!blocksHold.ContainsKey(color) || blocksHold[color] <= 0)
        {
            Debug.Log($"No more {color} blocks to place.");
            return;
        }

        // 确定放置方向
        float direction = (spriteRenderer != null && spriteRenderer.flipX) ? -1f : 1f;

        // 计算放置位置：基准点加上水平和垂直偏移量
        Vector3 placementPosition = buildPoint.position + new Vector3(direction * horizontalOffset, verticalOffset, 0);

        // 检测放置位置是否被占用
        Collider2D hit = Physics2D.OverlapCircle(placementPosition, placeRange, LayerMask.GetMask("Block"));
        if (hit == null)
        {
            GameObject newBlock;
            if (color == "black")
            {
                newBlock = Instantiate(blackBlockPrefab, placementPosition, Quaternion.identity);
            }
            else // color == "red"
            {
                newBlock = Instantiate(redBlockPrefab, placementPosition, Quaternion.identity);
            }

            placedBlocks[color].Add(newBlock); // 将新放置的方块添加到对应的列表中
            blocksHold[color] -= 1;            // 减少对应颜色的方块数量

            UpdateUI();
            Debug.Log($"{color} Block Placed at {placementPosition}!");
        }
        else
        {
            Debug.Log("Cannot place block here. Space is already occupied.");
        }
    }

    void UpdateUI()
    {
        blocksNum.text = blocksHold[current_block_type].ToString();

        if (current_block_type == "black")
        {
            block_color.sprite = blackBlockSprite;
        }
        else if (current_block_type == "red")
        {
            block_color.sprite = redBlockSprite;
        }
    }

    public void ResetBlocks()
    {
        // Destroy all placed blocks
        foreach (var blockList in placedBlocks.Values)
        {
            foreach (GameObject block in blockList)
            {
                Destroy(block);
            }
            blockList.Clear();
        }

        // Reset the block count for each type
        blocksHold["black"] = 10;
        blocksHold["red"] = 10;

        // Update the UI to reflect the reset counts
        UpdateUI();

        Debug.Log("All blocks cleared, and block count reset.");
    }

    private void OnDrawGizmosSelected()
    {
        if (buildPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(buildPoint.position + new Vector3(horizontalOffset, verticalOffset, 0), placeRange);
        }
    }
}
