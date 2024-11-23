using UnityEngine;

public class SpurBlock : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Call the Respawn method if it exists on the player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Respawn();
            }
            else
            {
                Debug.LogWarning("Respawn method not found on the Player!");
            }
        }
    }
}
