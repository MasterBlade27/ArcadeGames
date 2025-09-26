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

    [SerializeField]
    private int lives = 3;
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
        if (audioController != null)
        {
            for (int j = 0; j < audioController.musicSources.Count; j++)    //I will change this code later
            {
                audioController.musicSources[0].volume = 0;
                audioController.musicSources[1].volume = 0;
                audioController.musicSources[2].volume = 0;
            }
            audioController.audioSource2.PlayOneShot(audioController.gameOverSFX);
        }

        ReplayGo.SetActive(true);

        if(FindAnyObjectByType<PowerUp>())
            DelPowerUps();
    }

    private void Replay()
    {
        if (audioController != null)
            audioController.audioSource2.PlayOneShot(audioController.oneUpSFX);
        if (audioController != null)
        {
            foreach (AudioSource i in audioController.musicSources)
            {
                for (int j = 0; j < audioController.musicSources.Count; j++)
                {
                    if (j != 0)
                        i.volume = 0;
                }
            }

            BlockRespawn.clear = 0;
            for (int j = 0; j < audioController.musicSources.Count; j++)    //I will change this code later
            {
                audioController.musicSources[0].volume = 1;
                audioController.musicSources[1].volume = 0;
                audioController.musicSources[2].volume = 0;
            }
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
        if (lives == 0)
        {
            mip.enabled = false;
            if (Test)
                GamaOvar();

            ScoreName SN = FindAnyObjectByType<ScoreName>();
            SN.HighscoreRestart();

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
