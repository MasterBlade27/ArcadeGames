using UnityEngine;

public class shotMove : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float timeout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.gameReset += OnReset;
    }
    private void OnDisable()
    {
        Player.gameReset -= OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
        timeout += Time.deltaTime;

        if(timeout >= 2f)
        { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Centipede") || other.gameObject.CompareTag("Mushroom") || other.gameObject.CompareTag("Tick"))
        {
            Destroy(gameObject);
        }
    }

    private void OnReset()
    {
        Destroy(gameObject);
    }
}
