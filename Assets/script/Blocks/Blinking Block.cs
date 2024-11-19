using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingBlock : MonoBehaviour
{
    public float start_blinking_time = 2f; // Time after the game starts before blinking begins
    public float blinking_interval = 1f;  // Interval between blinks

    private bool isBlinking = false; // Whether blinking has started
    [SerializeField] private float acc_time = 0f;     // Serialized accumulated time for managing timing
    private bool isVisible = true;   // Current visibility state of the block

    private Renderer blockRenderer;  // Renderer of the block
    private BoxCollider2D blockCollider; // Reference to the BoxCollider2D component
    private bool isLevel = false;
    void Start()
    {
        // Get the Renderer and BoxCollider2D components of the block
        blockRenderer = GetComponent<Renderer>();
        blockCollider = GetComponent<BoxCollider2D>(); // Ensure a BoxCollider2D is attached to the block

        // Check if there is a GameObject with the LevelEnter component in the scene
        GameObject levelEnterObject = GameObject.Find("LevelEntered");

        // If LevelEnter component is found, start the blinking process
        if (levelEnterObject != null)
        {
            isLevel = true;
        }
    }

    void Update()
    {
        if (!isLevel)
            return;
            
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

        if (blockRenderer != null)
        {
            blockRenderer.enabled = isVisible; // Toggle the renderer's visibility (makes the object visible/invisible)
        }

        if (blockCollider != null)
        {
            blockCollider.enabled = isVisible; // Toggle the BoxCollider2D's enabled state
        }
    }

    void StartBlinking()
    {
        // This function will start the blinking process
        isBlinking = true;
        acc_time = 0; // Reset timer for blinking intervals
    }
}
