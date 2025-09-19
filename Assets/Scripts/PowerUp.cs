using UnityEngine;
using System;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;

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
                collision.gameObject.GetComponent<Paddle>().doubleSize();
            }
            else
            {
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

