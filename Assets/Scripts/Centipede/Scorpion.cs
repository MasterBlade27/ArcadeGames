using UnityEngine;

public class Scorpion : MonoBehaviour
{
    private scoretest scoretest => FindAnyObjectByType<scoretest>();
    [SerializeField]
    private float speed = 2f;
    private float timePassed = 0f;

    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();

        if (AC != null)
            AC.PlayRandom(AC.scorpionSounds, Random.Range(0, AC.scorpionSounds.Count), 5);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (timePassed >= 2f)
        {
            speed += 2f;
            timePassed = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            Debug.Log("Scorpion hit a mushroom!");
            other.GetComponent<Mushroom>().Poison = true;
        }
        if(other.CompareTag("Projectile"))
        {
            if (AC != null)
            {
                AC.StopVol(5);
                AC.PlayNormal(AC.scorpionHit, 5);
            }

            if (scoretest != null)
                scoretest.scoreUpdate(1000);
            Destroy(gameObject);
        }
    }
}
