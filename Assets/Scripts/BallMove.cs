using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour
{
    [SerializeField]
    private bool Demo, Multi;

    private Rigidbody RB;
    [SerializeField]
    private GameObject Paddle, OriBall;

    //Vel = Velocity
    [SerializeField]
    private Vector3 Vel;

    //PadDir = Paddle Direction
    [SerializeField]
    private Vector3 PadDir;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int Bounce, StuckBounce, BallAmt;
    private bool Stuck;
    public bool Starto;

    [SerializeField]
    private AudioController AC;

    private int ballPitch = 1;

    private PaddleMove pInput;

    private float test = 0;

    private bool oneHit = false;

    public static bool wait = false;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Starto = true;

        if (!Demo)
        {
            Paddle = FindAnyObjectByType<MoveInput>().gameObject;

            PowerUp.HalfSpeed += OnHalfSpeed;
            PowerUp.DoubleSpeed += OnDoubleSpeed;
            Restart.OnRestart += OnRestart;
            PowerUp.OneShot += OnOneHit;
        }

        if (Multi)
            foreach (BallMove BM in FindObjectsByType<BallMove>(FindObjectsSortMode.None))
                if (BM.Multi == false)
                    OriBall = BM.gameObject;
    }

    private void OnEnable()
    {
        if(!Demo)
        {
            pInput = new PaddleMove();
            pInput.Enable();
        }
    }

    private void OnDisable()
    {
        if (!Demo)
        {
            pInput.Disable();
            PowerUp.HalfSpeed -= OnHalfSpeed;
            PowerUp.DoubleSpeed -= OnDoubleSpeed;
            Restart.OnRestart -= OnRestart;
            PowerUp.OneShot -= OnOneHit;
        }
    }

    private void Update()
    {
        if (Starto && !Demo)
        {
            transform.position = Paddle.transform.position + Vector3.up * 0.5f;

            test += Time.deltaTime;
            test = Mathf.Clamp(test, 0, 1f);

            if ((pInput.Movement.Play.IsPressed() || Input.GetMouseButtonUp(0)) && test == 1)
            {
                //Starting Movement for the Ball
                Vector3 fforce = new Vector3(Paddle.transform.position.x / 7.5f, 1f, 0f);

                //Moves the Ball upon Starting
                RB.AddForce(fforce * 3f, ForceMode.VelocityChange);

                Starto = false;
            }
        }

        else if (Starto && Demo)
        {
            //Starting Movement for the Ball
            Vector3 fforce = new Vector3(Random.Range(-1f, 1f), 1f, 0f);

            //Moves the Ball upon Starting
            RB.AddForce(fforce * 3f, ForceMode.VelocityChange);

            Starto = false;
        }

        if (Starto && Multi)
        {
            //transform.position = OriBall.transform.position - Vector3.down * 0.5f;
            Bounce = OriBall.GetComponent<BallMove>().Bounce;
            speed = OriBall.GetComponent<BallMove>().speed;
            AC = OriBall.GetComponent<BallMove>().AC;
            Vel = OriBall.GetComponent<BallMove>().Vel;
            Vel.y = 1f;
            RB.linearVelocity = Vel.normalized * speed;

            Starto = false;
            Multi = false;
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
                fforce = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(0.5f, 0.75f), 0f);
            else
                fforce = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.5f, -0.75f), 0f);
            
            RB.AddForce(fforce * 2, ForceMode.VelocityChange);
        }

        BallAmt = FindObjectsByType<BallMove>(FindObjectsSortMode.InstanceID).Length;
    }

    //Test for if the Ball hits anything
    private void OnCollisionEnter(Collision collision)
    {
        //Local Variable, Direction the Ball needs to go in
        Vector3 direction;

        //Test for if the Ball hits the Paddle
        if (collision.gameObject == Paddle)
        {
            if (AC != null)
            {
                //Play sound when ball hits the paddle
                ballPitch = 0;
                AC.PlayBall(1, AC.paddleAudioClips, Random.Range(0, AC.paddleAudioClips.Count));
            }

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
            direction = Vector3.zero;

            if (BallAmt > 1)
            {
                Restart rs = GetComponent<Restart>();
                rs.MultiKill();
            }

            else
            {
                Restart rs = GetComponent<Restart>();
                rs.BallReset();

                Bounce = -1;
                StuckBounce = -1;
                speed = (5 + LevelThingIGuess.levelSpeed);
            }
        }

        else if (collision.gameObject.CompareTag("Block"))
        {
            if (AC != null)
            {
                //Play sound when ball hits the blocks
                if (ballPitch < 5)
                    ballPitch += 1;

                AC.PlayBall(ballPitch, AC.blockAudioClips, Random.Range(0, AC.blockAudioClips.Count));
            }

            if(oneHit)
            {
                ArmorBlock AB = collision.gameObject.GetComponent<ArmorBlock>();
                AB.DeleteBlock(Vel);
            }
            else
            {
                ArmorBlock AB = collision.gameObject.GetComponent<ArmorBlock>();
                AB.ArmorDestroy(Vel);
            }
            

            //The Ball will Bounce off the object in a Normal Reflective way
            direction = Vector3.Reflect(Vel.normalized, collision.contacts[0].normal);

            //Reset StuckBounce Counter
            StuckBounce = -1;
        }

        //Test if the Ball hits anything else
        else
        {
            if (AC != null)
            {
                //Play sound when ball hits the wall
                AC.PlayBall(1, AC.wallAudioClips, Random.Range(0, AC.wallAudioClips.Count));
            }

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
        if (StuckBounce % 6 == 0 && !(StuckBounce == 0))
        {
            //Activates Anti Stuck Correction
            Stuck = true;
        }

        //Ball's Speed increases on certain Bounces
        if (Bounce % 5 == 0 && !(Bounce == 0))
        {
            //Increase Speed
            speed += 0.4f;
        }
    }

    private Coroutine speedEffectCoroutine;
    private float originalSpeed;
    private bool isSpeedEffectActive = false;

    private void OnHalfSpeed()
    {
        StartSpeedEffect(0.5f, 5f); // 0.5x speed for 5 seconds
    }

    private void OnDoubleSpeed()
    {
        StartSpeedEffect(2f, 5f); // 2x speed for 5 seconds
    }

    private void StartSpeedEffect(float multiplier, float duration)
    {
        if (speedEffectCoroutine != null)
            StopCoroutine(speedEffectCoroutine);

        speedEffectCoroutine = StartCoroutine(SpeedEffectTimer(multiplier, duration));
    }

    private IEnumerator SpeedEffectTimer(float multiplier, float duration)
    {
        if (!isSpeedEffectActive)
        {
            originalSpeed = speed;
            speed *= multiplier;
            RB.linearVelocity = Vel.normalized * speed;
        }
        isSpeedEffectActive = true;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        RB.linearVelocity = Vel.normalized * speed;
        isSpeedEffectActive = false;
        speedEffectCoroutine = null;
    }

    private void OnRestart()
    {
        if (speedEffectCoroutine != null)
            StopCoroutine(speedEffectCoroutine);
        isSpeedEffectActive = false;
        oneHit = false;
    }

    private void OnOneHit(float duration)
    {
        oneHit = true;
        StartCoroutine(OneHitTimer(duration));
    }

    private IEnumerator OneHitTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        oneHit = false;
    }
    private void OnTriggerStay(Collider other)
    {
        wait = true;
    }
    private void OnTriggerExit(Collider other)
    {
        wait = false;
    }

    public int CheckBalls()
    {
        return BallAmt;
    }
}