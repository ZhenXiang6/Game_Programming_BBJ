using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 玩家
    public float smoothing = 5f;
    private Vector3 offset;

    // 摄像机边界
    public Vector2 minPosition;
    public Vector2 maxPosition;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;

        // 限制摄像机位置
        float posX = Mathf.Clamp(targetCamPos.x, minPosition.x, maxPosition.x);
        float posY = Mathf.Clamp(targetCamPos.y, minPosition.y, maxPosition.y);

        Vector3 clampedPosition = new Vector3(posX, posY, targetCamPos.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothing * Time.deltaTime);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minPosition.x, minPosition.y, 0), new Vector3(minPosition.x, maxPosition.y, 0));
        Gizmos.DrawLine(new Vector3(minPosition.x, maxPosition.y, 0), new Vector3(maxPosition.x, maxPosition.y, 0));
        Gizmos.DrawLine(new Vector3(maxPosition.x, maxPosition.y, 0), new Vector3(maxPosition.x, minPosition.y, 0));
        Gizmos.DrawLine(new Vector3(maxPosition.x, minPosition.y, 0), new Vector3(minPosition.x, minPosition.y, 0));
    }

}
