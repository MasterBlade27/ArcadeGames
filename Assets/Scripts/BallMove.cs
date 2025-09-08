using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Rigidbody RB;

    //Vel = Velocity
    [SerializeField]
    private Vector3 Vel;

    //PadDir = Paddle Direction
    [SerializeField]
    private Vector3 PadDir;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int Bounce;

    private bool Kill;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip paddleSFX;
    [SerializeField]
    private AudioClip blockSFX;
    [SerializeField]
    private AudioClip wallSFX;

    void Start()
    {
        //Starting Movement for the Ball
        Vector3 fforce = new Vector3(1f, 1f, 0f);
        RB = GetComponent<Rigidbody>();

        //Moves the Ball upon Starting
        RB.AddForce(fforce * 3f, ForceMode.VelocityChange);
    }

    private void Update()
    {
        //Sets Vel as Ball's Velocity
        Vel = RB.linearVelocity;

        //Corrects Ball's Speed if decreased due to Bouncing
        if(speed != Vel.magnitude)
        {
            //Course Correction
            RB.linearVelocity = Vel.normalized * speed;
        }
    }

    //Test for if the Ball hits anything
    private void OnCollisionEnter(Collision collision)
    {        
        //Local Variable, Direction the Ball needs to go in
        Vector3 direction;

        //Test for if the Ball hits the Paddle
        if (collision.gameObject.CompareTag("GameController"))
        {
            //Play sound when ball hits the paddle
            audioSource.PlayOneShot(paddleSFX);

            //Finds the angle between the Ball and the Paddle
            //Sends the Ball in that Direction
            PadDir = transform.position - collision.transform.position;

            //When hitting the Ball with a Paddle
            //The Ball will go in the Direction with the Paddle and not Bounce off
            direction = PadDir.normalized;
        }

        else if (collision.gameObject.CompareTag("Block"))
        {
            //Play sound when ball hits the blocks
            audioSource.PlayOneShot(blockSFX);

            ArmorBlock AB = collision.gameObject.GetComponent<ArmorBlock>();
            AB.ArmorDestroy();

            direction = Vector3.Reflect(Vel.normalized, collision.contacts[0].normal);
        }

        /*        else if (collision.gameObject.CompareTag("KillBox"))
                {
                    direction = Vector3.zero;
                    Kill = true;
                }*/


        //Test if the Ball hits anything else
        else
        {
            //Play sound when ball hits the wall
            audioSource.PlayOneShot(wallSFX);

            //The Ball will Bounce off the object in a Normal Reflective way
            direction = Vector3.Reflect(Vel.normalized, collision.contacts[0].normal);
        }

        //Corrects the Ball's Velocity
        RB.linearVelocity = direction * speed;
        //Adds 1 to the Bounce Counter
        Bounce++;

        //Ball's Speed increases on certain Bounces
        if (Bounce == 4 || Bounce == 12)
        {
            //Increase Speed
            speed += 3;
        }
    }
}
