using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour
{
    private Rigidbody RB;
    private GameObject Paddle;

    //Vel = Velocity
    [SerializeField]
    private Vector3 Vel;

    //PadDir = Paddle Direction
    [SerializeField]
    private Vector3 PadDir;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int Bounce, StuckBounce;
    private bool Stuck;

    private bool Kill;
    public bool Starto;

    [SerializeField]
    private AudioController audioController;
    [SerializeField]
    private AudioClip paddleSFX;
    [SerializeField]
    private AudioClip blockSFX;
    [SerializeField]
    private AudioClip wallSFX;

    private int ballPitch = 1;

    private bool isHalfSpeedActive = false;

    void Start()
    {
        RB = GetComponent<Rigidbody>();

        Starto = true;

        PowerUp.HalfSpeed += OnHalfSpeed;
    }

    private void OnDisable()
    {
        PowerUp.HalfSpeed -= OnHalfSpeed;
    }

    private void Update()
    {
        if (Starto)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //Starting Movement for the Ball
                Vector3 fforce = new Vector3(Random.Range(-1f, 1f), 1f, 0f);

                //Moves the Ball upon Starting
                RB.AddForce(fforce * 3f, ForceMode.VelocityChange);

                Starto = false;
            }

            else
            {
                transform.position = Paddle.transform.position + Vector3.up * 0.5f;
            }
        }

        //Sets Vel as Ball's Velocity
        Vel = RB.linearVelocity;

        //Corrects Ball's Speed if decreased due to Bouncing
        if(speed != Vel.magnitude)
        {
            //Course Correction
            RB.linearVelocity = Vel.normalized * speed;
        }

        //Anti Stuck Correction
        if(Stuck)
        {
            Stuck = false;
            Vector3 fforce;

            if(RB.linearVelocity.y > 0f)
                fforce = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(0.1f, 0.75f), 0f);
            else
                fforce = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.1f, -0.75f), 0f);
            
            RB.AddForce(fforce * 2, ForceMode.VelocityChange);
        }
    }

    //Test for if the Ball hits anything
    private void OnCollisionEnter(Collision collision)
    {        
        //Local Variable, Direction the Ball needs to go in
        Vector3 direction;

        //Test for if the Ball hits the Paddle
        if (collision.gameObject == Paddle)
        {
            //Play sound when ball hits the paddle
            audioController.audioSource1.pitch = 1;
            ballPitch = 0;
            audioController.audioSource1.PlayOneShot(paddleSFX);

            //Finds the angle between the Ball and the Paddle
            //Sends the Ball in that Direction
            PadDir = transform.position - collision.transform.position;

            //When hitting the Ball with a Paddle
            //The Ball will go in the Direction with the Paddle and not Bounce off
            direction = PadDir.normalized;

            //Reset StuckBounce Counter
            StuckBounce = -1;
        }

        else if (collision.gameObject.CompareTag("KillBox"))
        {
            Restart rs = GetComponent<Restart>();
            rs.BallReset();

            Bounce = -1;
            StuckBounce = -1;
            speed = 5;
            direction = Vector3.zero;
        }

        else if (collision.gameObject.CompareTag("Block"))
        {
            //Play sound when ball hits the blocks
            if (ballPitch < 5)
                ballPitch += 1;

            audioController.audioSource1.pitch = ballPitch;
            audioController.audioSource1.PlayOneShot(blockSFX);

            ArmorBlock AB = collision.gameObject.GetComponent<ArmorBlock>();
            AB.ArmorDestroy();

            //The Ball will Bounce off the object in a Normal Reflective way
            direction = Vector3.Reflect(Vel.normalized, collision.contacts[0].normal);

            //Reset StuckBounce Counter
            StuckBounce = -1;
        }

        //Test if the Ball hits anything else
        else
        {
            //Play sound when ball hits the wall
            audioController.audioSource1.pitch = 1;
            audioController.audioSource1.PlayOneShot(wallSFX);

            //The Ball will Bounce off the object in a Normal Reflective way
            direction = Vector3.Reflect(Vel.normalized, collision.contacts[0].normal);
        }

        //Corrects the Ball's Velocity
        RB.linearVelocity = direction * speed;
        //Adds 1 to the Bounce Counter
        Bounce++;
        //Adds 1 to the StuckBounce Counter
        StuckBounce++;

        //If the Ball Bounces 5 Times Without Touching Anything 
        if (StuckBounce % 8 == 0 && !(StuckBounce == 0))
        {
            //Activates Anti Stuck Correction
            Stuck = true;
        }

        //Ball's Speed increases on certain Bounces
        if (Bounce == 4 || Bounce == 12)
        {
            //Increase Speed
            speed += 3;
        }
    }
    private Coroutine halfSpeedCoroutine;
    private float originalSpeed;

    private void OnHalfSpeed()
    {
        if (halfSpeedCoroutine != null)
            StopCoroutine(halfSpeedCoroutine);

        halfSpeedCoroutine = StartCoroutine(HalfSpeedTimer(5f)); // 5 seconds as example
    }

    private IEnumerator HalfSpeedTimer(float duration)
    {
        originalSpeed = speed;

        if (!isHalfSpeedActive)
        {
            speed *= 0.5f;
            RB.linearVelocity = Vel.normalized * speed;
        }
        isHalfSpeedActive = true;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        RB.linearVelocity = Vel.normalized * speed;
        isHalfSpeedActive = false;
        halfSpeedCoroutine = null;
    }
}
