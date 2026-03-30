using UnityEngine;

public class Spider : MonoBehaviour
{
    private scoretest scoretest => FindAnyObjectByType<scoretest>();
    public LayerMask collisionMask;
    private float zMove = 1f;
    private float xMove = 1f;
    private float udChanceTimer = 0f;
    private float udTimer = 1.5f;
    private bool ud = false;
    public float speed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.gameReset += onReset;
    }
    private void OnDisable()
    {
        Player.gameReset -= onReset;
    }

    // Update is called once per frame
    void Update()
    {
        float movespeed = (MushroomScoreMarch.regenerating ? 0f : speed) * Time.deltaTime;
        udChanceTimer += Time.deltaTime;
        if (udChanceTimer > 3f)
        {
            udChanceTimer = 0f;
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < 0.3f)
            {
                ud = true;
            }
        }
        if (ud)
        { 
            udTimer -= Time.deltaTime;
            transform.position += new Vector3(0, 0, zMove * 1.5f) * movespeed;
            if (udTimer <= 0f)
            {
                ud = false;
                udTimer = 1.5f;
            }
        }
        else
            transform.position += new Vector3(xMove, 0, zMove) * Time.deltaTime * 20f;
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo ,0.5f, collisionMask))
        {
            Destroy(hitinfo.transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (scoretest != null)
                scoretest.scoreUpdate(200);
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "sBarrier")
        {
            zMove = -zMove;
        }
        else if (collision.gameObject.name == "LeftBorder" || collision.gameObject.name == "RightBorder")
        {
            xMove = -xMove;
        }
    }

    private void onReset()
    {
        Destroy(gameObject);
    }

}
