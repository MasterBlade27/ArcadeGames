using UnityEngine;
using System;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;

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
        transform.position += Vector3.down * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            float pUp = UnityEngine.Random.Range(0, 2);
            if (pUp == 1)
            {
                powerUp.PlayOneShot(largePaddle);
                collision.gameObject.GetComponent<Paddle>().doubleSize();
            }
            else
            {
                powerUp.PlayOneShot(slowTime);
                HalfSpeed();
            }

            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("KillBox"))
        {
            Destroy(gameObject);
        }
    }
    private void ResetPups()
    {
        Destroy(gameObject);
    }
}

