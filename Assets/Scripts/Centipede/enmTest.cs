using UnityEngine;

public class enmTest : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private bool forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        forward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 29)
        {
            forward = false;
            transform.position += new Vector3(0f, 0f, -1f);
        }
        if(transform.position.x <= 0)
        {
            forward = true; 
            transform.position += new Vector3(0f, 0f, -1f);
        }
        if (forward)
        {
            transform.position += new Vector3(Time.deltaTime * speed, 0f, 0f);
        }
        if (!forward)
        {
            transform.position += new Vector3(Time.deltaTime * (speed * -1), 0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collided with: " + collision.transform.name);
        if (collision.transform.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
