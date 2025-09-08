using UnityEngine;

public class Paddle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Coroutine sizeCoroutine;

    public void doubleSize()
    {
        if (sizeCoroutine != null)
            StopCoroutine(sizeCoroutine);
        sizeCoroutine = StartCoroutine(DoubleSizeRoutine());
    }

    private System.Collections.IEnumerator DoubleSizeRoutine()
    {
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
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y, transform.position.z);
            yield return null;
        }

        // Restore original size
        transform.localScale = new Vector3(2f, transform.localScale.y, transform.localScale.z);
        sizeCoroutine = null;
    }
    /*public void doubleSize()
    {
        float originalX = transform.localScale.x;
        float sizeTime = 5f;
        transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
        while (sizeTime > 0)
        {
            Debug.Log(sizeTime);
            sizeTime -= Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y, transform.position.z);
        }
        transform.localScale = new Vector3(originalX, transform.localScale.y, transform.localScale.z);
    }*/
}
