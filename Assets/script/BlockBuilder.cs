using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlockBuilder : MonoBehaviour
{
    [Header("放置方块参数")]
    public GameObject blackBlockPrefab; // 黑色方块预制体
    public GameObject redBlockPrefab;   // 红色方块预制体
    public Transform buildPoint;        // 方块放置的位置
    public float placeRange = 0.5f;     // 放置范围
    public float verticalOffset = -1f;  // 角色脚下的垂直偏移量

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
    void Start() 
    {
        current_block_type = block_types[0]; // Initialize the current block type
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
        currentIndex = (currentIndex + 1) % block_types.Length; // Cycle to the next block type
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

        Vector3 placementPosition = buildPoint.position + new Vector3(0, verticalOffset, 0);

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

            placedBlocks[color].Add(newBlock); // Add to the list for the specified color
            blocksHold[color] -= 1;            // Decrement the count for that color

            UpdateUI();
            Debug.Log($"{color} Block Placed!");
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

    private void OnDrawGizmosSelected()
    {
        if (buildPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(buildPoint.position + new Vector3(0, verticalOffset, 0), placeRange);
        }
    }
}
