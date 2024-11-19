using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostBlock : MonoBehaviour
{
    public float boostedJumpForce = 20f; // Jump force when boosted
    public float left_timer = 0f; // Timer to track how long the player has been away
    private bool isCounting = false; // Prevent multiple coroutines running

    private void OnCollisionEnter2D(Collision2D collision)
    {
        left_timer = 0f; // Reset the timer
        isCounting = false; // Stop counting if the player comes back
        // Check if the object colliding with the block has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player touched the solid Jump Boost Block!");

            // Access the player's script handling jump
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Set the player's jump force to boosted
                playerController.SetJumpForce(boostedJumpForce);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the object exiting the collision has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player left the solid Jump Boost Block!");

            // Access the player's script handling jump
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null && !isCounting)
            {
                // Start coroutine to count left time and reset jump force
                StartCoroutine(CountLeftTime(playerController, 0.5f));
            }
        }
    }

    // Coroutine to count left time and reset jump force
    private IEnumerator CountLeftTime(PlayerController playerController, float delay)
    {
        isCounting = true; // Mark that counting has started
        while (left_timer < delay)
        {
            left_timer += Time.deltaTime; // Increment timer
            yield return null; // Wait for the next frame
        }

        // After delay is reached, reset the jump force
        playerController.SetJumpForce(-1f); // Reset to default
        Debug.Log("Player's jump force reset to normal!");
        isCounting = false; // Allow new counting
    }
}
