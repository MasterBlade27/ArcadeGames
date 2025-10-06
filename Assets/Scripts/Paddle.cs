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
        MoveInput MI = FindAnyObjectByType<MoveInput>();

        float originalX = transform.localScale.x;
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
                MI.clamping = 6.5f;
            if (half)
                MI.clamping = 8f;
            yield return null;
        }

        // Restore original size
        transform.localScale = new Vector3(originalX, transform.localScale.y, transform.localScale.z);
        MI.clamping = 7.5f;
        half = false;
        sizeCoroutine = null;
    }
}
