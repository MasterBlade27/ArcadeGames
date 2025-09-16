using System.Collections;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private GameObject Ball;
    [SerializeField]
    private Vector3 StopPos;
    private Coroutine Reseting;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private TextMeshProUGUI livetext;

    [SerializeField]
    private GameObject ReplayGo;

    private MoveInput mip;

    private void Start()
    {
        mip = FindAnyObjectByType<MoveInput>();

        Ball = gameObject;

        ReplayGo.SetActive(false);
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
        Ball.GetComponent<BallMove>().Starto = true;
    }

    private void GamaOvar()
    {
        ReplayGo.SetActive(true);
        mip.enabled = false;
    }

    private void Update()
    {
        livetext.text = lives.ToString();

        if (Input.GetKeyUp(KeyCode.Space) && ReplayGo.activeSelf)
        {
            Restarto();
        }
    }

    private void Restarto()
    {
        lives = 3;
        ReplayGo.SetActive(false);
        mip.enabled = true;

        BlockRespawn BR = FindAnyObjectByType<BlockRespawn>();
        BR.TempRestart();
        Scoring SC = FindAnyObjectByType<Scoring>();
        SC.ResetScore();

        Ball.GetComponent<BallMove>().Starto = true;
    }

    private IEnumerator ResetEnum()
    {
        if(lives == 0)
        {
            GamaOvar();
            yield break;
        }

        float endtime = .5f;
        float timer = 0f;
        while (timer < endtime)
        {
            timer += Time.deltaTime;
            Ball.transform.position = StopPos;
            Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            if (timer >= endtime)
            {
                StartAgain();
                Reseting = null;
            }

            yield return null;
        }
    }
}
