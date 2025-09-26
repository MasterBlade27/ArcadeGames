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
    private AudioController audioController;

    private PaddleMove pInput;

    public static bool isGameOver = false;

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
        isGameOver = false;

        if(audioController != null)
            audioController.audioSource2.PlayOneShot(audioController.floorSFX);

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
        isGameOver = true;
        if (audioController != null)
        {
            for (int i = 0; i < audioController.musicSources.Count; i++)    //I will change this code later
                audioController.musicSources[i].volume = 0;
            audioController.audioSource2.PlayOneShot(audioController.gameOverSFX);
        }

        ReplayGo.SetActive(true);

        if(FindAnyObjectByType<PowerUp>())
            DelPowerUps();
    }

    private void Replay()
    {
        if (audioController != null)
        {
            for (int i = 0; i < audioController.musicSources.Count; i++)
                audioController.musicSources[0].volume = 1;
                audioController.musicSources[1].volume = 0;
                audioController.musicSources[2].volume = 0;
            audioController.audioSource2.PlayOneShot(audioController.oneUpSFX);
        }

        lives = totallives;
        ReplayGo.SetActive(false);
        mip.enabled = true;
        mip.gameObject.transform.position = OriPad;

        BlockRespawn BR = FindAnyObjectByType<BlockRespawn>();
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
