using UnityEngine;
using System;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;
    public static event Action DoubleSpeed;
    public static event Action<int, float> ScoreMultiply;
    public static event Action<float> OneShot;
    public static event Action OneUp;

    public int pUp;

    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private AudioController AC;
    [SerializeField]
    private GameObject Ball;
    private PowerUpHUD PUHUD;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        PUHUD = FindAnyObjectByType<PowerUpHUD>();
        //pUp = UnityEngine.Random.Range(0, AC.powerupClips.Count);

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
                Debug.Log("Large Paddle");
                collision.gameObject.GetComponent<Paddle>().doubleSize(true);
                Destroy(gameObject);

            }
            else if (pUp == 1)
            {
                Debug.Log("Small Paddle");
                collision.gameObject.GetComponent<Paddle>().doubleSize(false);
                Destroy(gameObject);
            }
            else if (pUp == 2)
            {
                Debug.Log("Slow Ball");
                HalfSpeed?.Invoke();
                Destroy(gameObject);

            }
            else if (pUp == 3)
            {
                Debug.Log("Fast Ball");
                DoubleSpeed?.Invoke();
                Destroy(gameObject);
            }
            else if (pUp == 4)
            {
                Debug.Log("Score Up");
                ScoreMultiply(2, 5f);
                Destroy(gameObject);
            }
            else if (pUp == 5)
            {
                Debug.Log("Oneshot");
                OneShot?.Invoke(5f);
                Destroy(gameObject);
            }
            else if (pUp == 6)
            {
                Debug.Log("Multiball");
                Instantiate(Ball, (transform.position + Vector3.up), Quaternion.identity);
                Destroy(gameObject);
            }
            else if (pUp == 7)
            {
                Debug.Log("1UP");
                OneUp?.Invoke();
                Destroy(gameObject);
            }

            AC.PlayVol(AC.powerupClips, pUp, 2f);
            PUHUD.Appear(pUp);
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

