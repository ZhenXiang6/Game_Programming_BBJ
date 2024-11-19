using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Start the coroutine to handle the delayed disappearance
        StartCoroutine(DisappearAfterDelay(1f)); // 1 second delay
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
        yield return new WaitForSeconds(delay);
        // After the delay, deactivate the block
        gameObject.SetActive(false);
    }
}
