using UnityEngine;

public class Scorpion : MonoBehaviour
{
    private scoretest scoretest => FindAnyObjectByType<scoretest>();
    [SerializeField]
    private float speed = 2f;
    private float timePassed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            if (scoretest != null)
                scoretest.scoreUpdate(500);
            Destroy(gameObject);
        }
    }
}
