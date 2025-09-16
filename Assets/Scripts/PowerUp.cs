using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            collision.gameObject.GetComponent<Paddle>().doubleSize();
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("KillBox"))
        {
            Destroy(gameObject);
        }
    }
}

