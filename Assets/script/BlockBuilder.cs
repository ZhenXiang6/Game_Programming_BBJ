using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("放置方块参数")]
    public GameObject blockPrefab; // 方块预制体
    public Transform buildPoint;    // 方块放置的位置
    public float placeRange = 0.5f;   // 放置范围
    public float verticalOffset = -1f; // 角色脚下的垂直偏移量

    private GameObject lastPlacedBlock; // 用于跟踪最后一个放置的方块

    void Update()
    {
        // 检测按下 E 键
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaceBlock();
        }
    }

    void PlaceBlock()
    {
        // 计算放置方块的位置，位于角色脚下一定距离
        Vector3 placementPosition = buildPoint.position + new Vector3(0, verticalOffset, 0);

        // 检查放置范围内是否已有方块
        Collider2D hit = Physics2D.OverlapCircle(placementPosition, placeRange, LayerMask.GetMask("Block"));
        if (hit == null)
        {
            // 如果已经有一个方块被放置，销毁它
            if (lastPlacedBlock != null)
            {
                Destroy(lastPlacedBlock);
            }

            // 实例化新方块并更新 lastPlacedBlock 引用
            lastPlacedBlock = Instantiate(blockPrefab, placementPosition, Quaternion.identity);
            Debug.Log("Block Placed!");
        }
        else
        {
            Debug.Log("Cannot place block here. Space is already occupied.");
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
