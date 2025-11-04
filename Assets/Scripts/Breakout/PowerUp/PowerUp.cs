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
        float Dura = 0;
        if (collision.gameObject.CompareTag("GameController"))
        {
            if (pUp == 0)
            {
                Debug.Log("LargePaddle");
                collision.gameObject.GetComponent<Paddle>().doubleSize(true);
                Destroy(gameObject);
                Dura = 10f;
            }
            else if (pUp == 1)
            {
                Debug.Log("SmallPaddle");
                collision.gameObject.GetComponent<Paddle>().doubleSize(false);
                Destroy(gameObject);
                Dura = 10f;
            }
            else if (pUp == 2)
            {
                Debug.Log("SlowBall");
                HalfSpeed?.Invoke();
                Destroy(gameObject);
                Dura = 20f;
            }
            else if (pUp == 3)
            {
                Debug.Log("FastBall");
                DoubleSpeed?.Invoke();
                Destroy(gameObject);
                Dura = 5f;
            }
            else if (pUp == 4)
            {
                Debug.Log("DoubleScore");
                ScoreMultiply(2, 20f);
                Destroy(gameObject);
                Dura = 20f;
            }
            else if (pUp == 5)
            {
                Debug.Log("OneShot");
                OneShot?.Invoke(20f);
                Destroy(gameObject);
                Dura = 20f;
            }
            else if (pUp == 6)
            {
                Debug.Log("MultiBall");
                Instantiate(Ball, (transform.position + Vector3.up), Quaternion.identity);
                Destroy(gameObject);
            }
            else if (pUp == 7)
            {
                Debug.Log("ExtraLife");
                OneUp?.Invoke();
                Destroy(gameObject);
            }

            FindAnyObjectByType<Scoring>().AddPuPScore();
            PUHUD.Appear(pUp, Dura);
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

