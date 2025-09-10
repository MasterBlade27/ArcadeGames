using System.Collections;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private GameObject Ball;
    [SerializeField]
    private Vector3 BallOri;
    [SerializeField]
    private Vector3 StopPos;
    private Coroutine Reseting;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private TextMeshProUGUI livetext;

    private void Start()
    {
        Ball = gameObject;
        BallOri = Ball.transform.position;

        livetext.text = lives.ToString();
    }

    public void BallReset()
    {
        lives--;

        StopPos = Ball.transform.position;
        if (Reseting != null)
            StopCoroutine(Reseting);
        Reseting = StartCoroutine(ResetEnum());
    }

    private void StartAgain()
    {
        Rigidbody RB = Ball.GetComponent<Rigidbody>();

        //Starting Movement for the Ball
        Vector3 fforce = new Vector3(1f, 1f, 0f);

        //Moves the Ball upon Starting
        RB.AddForce(fforce * 3f, ForceMode.VelocityChange);
    }

    private void GamaOvar()
    {

    }

    private IEnumerator ResetEnum()
    {
        livetext.text = lives.ToString();

        if(lives == 0)
        {
            GamaOvar();
            yield break;
        }

        float endtime = 1f;
        float timer = 0f;
        while (timer < endtime)
        {
            timer += Time.deltaTime;
            Ball.transform.position = StopPos;
            Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            if (timer >= endtime)
            {
                Ball.transform.position = BallOri;
                StartAgain();
                Reseting = null;
            }

            yield return null;
        }
    }
}
