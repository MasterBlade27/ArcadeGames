using UnityEngine;
using System;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;

    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private AudioSource powerUp;
    [SerializeField]
    private AudioClip slowTime;
    [SerializeField]
    private AudioClip largePaddle;
    

    private void Start()
    {
        Restart.DelPowerUps += ResetPups;
    }
    private void OnDisable()
    {
        Restart.DelPowerUps -= ResetPups;
    }
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * fallSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            float pUp = UnityEngine.Random.Range(0, 2);
            if (pUp == 1)
            {
                powerUp.PlayOneShot(largePaddle, 10f);
                collision.gameObject.GetComponent<Paddle>().doubleSize();
                StartCoroutine(DestroyAfterSFX(largePaddle.length));
            }
            else
            {
                powerUp.PlayOneShot(slowTime, 10f);
                HalfSpeed?.Invoke();
                StartCoroutine(DestroyAfterSFX(slowTime.length));
            }
        }
        else if (collision.gameObject.CompareTag("KillBox"))
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyAfterSFX(float delay)
    {
        if (gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mrender))
            mrender.enabled = false;
        if (gameObject.TryGetComponent<Collider>(out Collider collider))
            collider.enabled = false;
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    private void ResetPups()
    {
        Destroy(gameObject);
    }
}

