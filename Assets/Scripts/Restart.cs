using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private bool Test;

    private GameObject Ball;
    private Vector3 StopPos;
    private Coroutine Reseting;

    public int lives = 3;
    private int totallives;

    [SerializeField]
    private TextMeshProUGUI livetext;

    [SerializeField]
    private GameObject ReplayGo;

    private MoveInput mip;
    private Vector3 OriPad;

    public static event Action DelPowerUps;

    [SerializeField]
    private AudioController AC;

    private PaddleMove pInput;

    private void Start()
    {
        totallives = lives;

        mip = FindAnyObjectByType<MoveInput>();
        OriPad = mip.transform.position;

        Ball = gameObject;

        ReplayGo.SetActive(false);
    }

    private void OnEnable()
    {
        pInput = new PaddleMove();
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    private void Update()
    {
        livetext.text = lives.ToString();

        if (pInput.Movement.Play.IsPressed() && ReplayGo.activeSelf && Test)
        {
            Replay();
        }
    }

    public void BallReset()
    {
        if(AC != null)
            AC.PlaySound(AC.floorSFX);

        lives--;

        StopPos = Ball.transform.position;

        if (FindAnyObjectByType<PowerUp>())
            DelPowerUps();

        if (Reseting != null)
            StopCoroutine(Reseting);
        Reseting = StartCoroutine(ResetEnum());
    }

    private void StartAgain()
    {
        Ball.GetComponent<BallMove>().Starto = true;
    }

    public void GamaOvar()
    {
        if (AC != null)
        {
            AC.ResetMusic();
            AC.PlaySound(AC.gameOverSFX);
        }

        ReplayGo.SetActive(true);

        if(FindAnyObjectByType<PowerUp>())
            DelPowerUps();
    }

    private void Replay()
    {
        if (AC != null)
        {
            AC.StartMusic(0);

            AC.PlaySound(AC.oneUpSFX);
        }

        lives = totallives;
        ReplayGo.SetActive(false);
        mip.enabled = true;
        mip.gameObject.transform.position = OriPad;

        BlockRespawn BR = FindAnyObjectByType<BlockRespawn>();
        BR.ResetLevels();
        BR.TempRestart();
        Scoring SC = FindAnyObjectByType<Scoring>();
        SC.ResetScore();

        Ball.GetComponent<BallMove>().Starto = true;
    }

    private IEnumerator ResetEnum()
    {
        float endtime = .5f;
        float timer = 0f;
        while (timer < endtime)
        {
            timer += Time.deltaTime;
            Ball.transform.position = StopPos;
            Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            if (timer >= endtime)
            {
                if (lives <= 0)
                {
                    mip.enabled = false;
                    if (Test)
                        GamaOvar();

                    ScoreName SN = FindAnyObjectByType<ScoreName>();
                    SN.HighscoreRestart();

                    yield break;
                }

                StartAgain();
                Reseting = null;
            }

            yield return null;
        }
    }
}
