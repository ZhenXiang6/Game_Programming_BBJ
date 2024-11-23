using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Start the coroutine to handle the delayed disappearance
        StartCoroutine(DisappearAfterDelay(1f)); // 1 second delay
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);
        
        // After the delay, deactivate the block
        gameObject.SetActive(false);
    }
}
