using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Coroutine sizeCoroutine;

    public void doubleSize()
    {
        if (sizeCoroutine != null)
            StopCoroutine(sizeCoroutine);
        sizeCoroutine = StartCoroutine(DoubleSizeRoutine());
    }

    private System.Collections.IEnumerator DoubleSizeRoutine()
    {
        MoveInput MI = FindAnyObjectByType<MoveInput>();

        float originalX = transform.localScale.x;
        float doubledX = 4;
        float sizeTime = 5f;

        // Double the size
        transform.localScale = new Vector3(doubledX, transform.localScale.y, transform.localScale.z);

        // Wait for the duration
        float timer = 0f;
        while (timer < sizeTime)
        {
            timer += Time.deltaTime;
            // Optionally clamp position here if needed
            MI.clamping = 6.5f;
            yield return null;
        }

        // Restore original size
        transform.localScale = new Vector3(2f, transform.localScale.y, transform.localScale.z);
        MI.clamping = 7.5f;
        sizeCoroutine = null;
    }
}
