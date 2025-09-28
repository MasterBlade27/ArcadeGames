using UnityEngine;
using System;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public static event Action HalfSpeed;

    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();

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
            int pUp = UnityEngine.Random.Range(0, 2);
            if (pUp == 1)
            {
                collision.gameObject.GetComponent<Paddle>().doubleSize();
            }
            else
            {
                HalfSpeed?.Invoke();
            }

            if (AC != null)
            {
                AC.PlayVol(AC.powerupClips, pUp, 2f);
                StartCoroutine(DestroyAfterSFX(AC.powerupClips[pUp].length));
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

