using UnityEngine;

public class HappyJelly : MonoBehaviour
{ 
    private float bounceForce = 10f;  // 彈開的力量

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查是否碰撞到玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // 計算彈開的方向，根據碰撞點施加彈力
                Vector2 bounceDirection = collision.contacts[0].normal;
                playerRb.AddForce(-bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
