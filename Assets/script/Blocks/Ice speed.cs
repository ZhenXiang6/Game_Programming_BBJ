using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespeed : MonoBehaviour
{
    public float left_timer = 0f; // Timer to track how long the player has been away
    private bool isCounting = false; // Prevent multiple coroutines running
    private float normal_speed_force;
    public float boosted_speed_force = 2f;

    private GameObject Player;

    void Start() 
    {
        // Find the Player GameObject and its PlayerController component
        Player = GameObject.FindWithTag("Player");
        if (Player != null)
        {
            PlayerController playerController = Player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                normal_speed_force = playerController.speed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding with the block has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player touched the Ice Speed Block!");

            // Access the player's script handling speed
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null)
            {  
                // Boost the player's speed
                playerController.speed = normal_speed_force * boosted_speed_force;
            }

            // Reset the timer and stop counting
            left_timer = 0f;
            isCounting = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the object exiting the collision has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player left the Ice Speed Block!");

            // Access the player's script handling speed
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null && !isCounting)
            {
                // Start coroutine to count left time and reset speed
                StartCoroutine(CountLeftTime(playerController, 1f));
            }
        }
    }

    // Coroutine to count left time and reset speed
    private IEnumerator CountLeftTime(PlayerController playerController, float delay)
    {
        isCounting = true; 
        left_timer = 0f; // Reset timer at the start
        while (left_timer < delay)
        {
            left_timer += Time.deltaTime;
            yield return null;
        }

        // Reset speed to normal
        if (playerController != null)
        {
            playerController.speed = normal_speed_force;
            Debug.Log("Player's speed reset to normal!");
        }
        isCounting = false; 
    }
}
