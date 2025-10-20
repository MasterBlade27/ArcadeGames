using UnityEngine;
using System;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;
    public static event Action DoubleSpeed;
    public static event Action<int, float> ScoreMultiply;
    public static event Action<float> OneShot;

    private int pUp;

    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        pUp = UnityEngine.Random.Range(0, 6);

        Restart.OnRestart += ResetPups;
    }
    private void OnDisable()
    {
        Restart.OnRestart -= ResetPups;
    }
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * fallSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            if (pUp == 0)
            {
                collision.gameObject.GetComponent<Paddle>().doubleSize(true);
                Destroy(gameObject);

            }
            else if (pUp == 1)
            {
                collision.gameObject.GetComponent<Paddle>().doubleSize(false);
                Destroy(gameObject);
            }
            else if (pUp == 2)
            {
                HalfSpeed?.Invoke();
                Destroy(gameObject);

            }
            else if (pUp == 3)
            {
                DoubleSpeed?.Invoke();
                Destroy(gameObject);
            }
            else if (pUp == 4)
            {
                ScoreMultiply(2, 5f);
                Destroy(gameObject);
            }
            else
            {
                OneShot?.Invoke(5f);
                Destroy(gameObject);
            }

            AC.PlayVol(AC.powerupClips, pUp, 2f);
            StartCoroutine(DestroyAfterSFX(AC.powerupClips[pUp].length));
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

