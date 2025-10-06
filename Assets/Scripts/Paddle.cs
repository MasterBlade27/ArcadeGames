using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Coroutine sizeCoroutine;

    public void doubleSize(bool big)
    {
        if (sizeCoroutine != null)
            StopCoroutine(sizeCoroutine);
        sizeCoroutine = StartCoroutine(DoubleSizeRoutine(big));
    }
    public bool half;

    private System.Collections.IEnumerator DoubleSizeRoutine(bool big)
    {
        float doubledX = 4;
        float halfX = 1;
        float sizeTime = 5f;

        // Double the size
        if (big)
            transform.localScale = new Vector3(doubledX, transform.localScale.y, transform.localScale.z);
        else
        {
            transform.localScale = new Vector3(halfX, transform.localScale.y, transform.localScale.z);
            half = true;
        }
        // Wait for the duration
        float timer = 0f;
        while (timer < sizeTime)
        {
            timer += Time.deltaTime;
            // Optionally clamp position here if needed
            if(big)
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y, transform.position.z);
            if(half)
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8f, 8f), transform.position.y, transform.position.z);
            yield return null;
        }

        // Restore original size
        transform.localScale = new Vector3(2f, transform.localScale.y, transform.localScale.z);
        half = false;
        sizeCoroutine = null;
    }
}
