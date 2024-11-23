using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Required for working with Tilemaps

public class BlinkingBlock : MonoBehaviour
{
    public float start_blinking_time = 2f; // Time after the game starts before blinking begins
    public float blinking_interval = 1f;  // Interval between blinks

    private bool isBlinking = false; // Whether blinking has started
    [SerializeField] private float acc_time = 0f; // Serialized accumulated time for managing timing
    private bool isVisible = true;   // Current visibility state of the block

    private Renderer blockRenderer;        // Renderer of the block
    private TilemapCollider2D tilemapCollider; // Reference to the TilemapCollider2D component
    private Tilemap tilemap;               // Reference to the Tilemap component

    void Start()
    {
        // Get the Renderer and TilemapCollider2D components of the block
        blockRenderer = GetComponent<Renderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>(); // Ensure a TilemapCollider2D is attached to the block
        tilemap = GetComponent<Tilemap>(); // Tilemap component for handling visual updates

        if (tilemapCollider == null)
        {
            Debug.LogError("No TilemapCollider2D found! Please attach one to the GameObject.");
        }
    }

    void Update()
    {       
        acc_time += Time.deltaTime;

        // Check if we have waited long enough to start blinking
        if (!isBlinking && acc_time >= start_blinking_time)
        {
            isBlinking = true;
            acc_time = 0; // Reset timer for blinking intervals
        }

        // If blinking has started, toggle active state based on the interval
        if (isBlinking && acc_time >= blinking_interval)
        {
            ToggleVisibility();
            acc_time = 0; // Reset timer for the next blink
        }
    }

    void ToggleVisibility()
    {
        isVisible = !isVisible; // Toggle visibility state

        if (tilemap != null)
        {
            // Show or hide the TilemapRenderer
            tilemap.color = isVisible ? Color.white : new Color(1f, 1f, 1f, 0f); // Set full transparency when invisible
        }

        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = isVisible; // Enable or disable the TilemapCollider2D
        }
    }

    void StartBlinking()
    {
        // This function will start the blinking process
        isBlinking = true;
        acc_time = 0; // Reset timer for blinking intervals
    }
}
