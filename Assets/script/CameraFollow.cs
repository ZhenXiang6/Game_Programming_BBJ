using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target; // 玩家
    public float smoothing = 5f; // 平滑度
    public Vector3 offset = new Vector3(-3f, -2f, -10f); // 摄像机偏移量

    [Header("摄像机边界")]
    public Vector2 minPosition = new Vector2(-10f, -10f);
    public Vector2 maxPosition = new Vector2(10f, 10f);

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Target 未设置。请将 Player 拖拽到 Target 字段。");
            return;
        }

        // 设置摄像机的初始位置
        transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetCamPos = target.position + offset;

        // 限制摄像机位置在边界内
        float posX = Mathf.Clamp(targetCamPos.x, minPosition.x, maxPosition.x);
        float posY = Mathf.Clamp(targetCamPos.y, minPosition.y, maxPosition.y);

        Vector3 clampedPosition = new Vector3(posX, posY, transform.position.z);

        // 平滑移动摄像机到目标位置
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothing * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 绘制摄像机边界框
        Gizmos.DrawLine(new Vector3(minPosition.x, minPosition.y, 0), new Vector3(minPosition.x, maxPosition.y, 0));
        Gizmos.DrawLine(new Vector3(minPosition.x, maxPosition.y, 0), new Vector3(maxPosition.x, maxPosition.y, 0));
        Gizmos.DrawLine(new Vector3(maxPosition.x, maxPosition.y, 0), new Vector3(maxPosition.x, minPosition.y, 0));
        Gizmos.DrawLine(new Vector3(maxPosition.x, minPosition.y, 0), new Vector3(minPosition.x, minPosition.y, 0));
    }
}
