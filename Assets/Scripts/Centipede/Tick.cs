using UnityEngine;
using System.Collections;

public class Tick : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private GameObject Mushroom;
    private float moveTarget = -1;
    private int curZ;
    public LayerMask collisionMask;
    private AudioController AC;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();

        if (AC != null)
            AC.PlayRandom(AC.tickSounds, Random.Range(0, AC.tickSounds.Count), 3);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.5f, moveTarget), moveSpeed);
        if (transform.position.z % 1 == 0)
        {
            moveTarget = transform.position.z - 1;

            curZ = (int)Mathf.Round(transform.position.z);
            if(curZ <= -39)
            {
                Destroy(gameObject);
            }
            if(Physics.OverlapBox(transform.position, new Vector3(0, 0, 0), Quaternion.identity, collisionMask).Length == 0)
            {
                if ((Random.Range(0f, 1f) < 0.2f) && curZ > -38)
                {
                    Instantiate(Mushroom, new Vector3(transform.position.x, 0.5f, curZ), Quaternion.identity);
                }
            }
        }

    }
    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }*/
    private void OnTriggerEnter(Collider collision)
    { 
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (AC != null)
                AC.PlayNormal(AC.tickHit, 3);
            Destroy(gameObject);
        }
    }
}
