using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{

    private GameObject Ball;
    private Vector3 StopPos;
    private Coroutine Reseting;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private TextMeshProUGUI livetext;

    [SerializeField]
    private GameObject ReplayGo;

    private MoveInput mip;
    private Vector3 OriPad;

    public static event Action DelPowerUps;

    [SerializeField]
    private AudioController audioController;

    private void Start()
    {
        mip = FindAnyObjectByType<MoveInput>();
        OriPad = mip.transform.position;

        Ball = gameObject;

        ReplayGo.SetActive(false);
    }

    public void BallReset()
    {
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

    private void GamaOvar()
    {
        for (int j = 0; j < audioController.musicSources.Count; j++)    //I will change this code later
        {
            audioController.musicSources[0].volume = 0;
            audioController.musicSources[1].volume = 0;
            audioController.musicSources[2].volume = 0;
            audioController.musicSources[3].volume = 0;
            audioController.musicSources[4].volume = 0;
        }
        audioController.audioSource2.PlayOneShot(audioController.gameOverSFX);

        ReplayGo.SetActive(true);
        mip.enabled = false;

        if(FindAnyObjectByType<PowerUp>())
            DelPowerUps();
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
        /*
        foreach (AudioSource i in audioController.musicSources)
        {
            for (int j = 0; j < audioController.musicSources.Count; j++)
            {
                if (j != 0)
                    i.volume = 0;
            }
        }
        */
        BlockRespawn.clear = 0;
        for (int j = 0; j < audioController.musicSources.Count; j++)    //I will change this code later
        {
            audioController.musicSources[0].volume = 1;
            audioController.musicSources[1].volume = 0;
            audioController.musicSources[2].volume = 0;
            audioController.musicSources[3].volume = 0;
            audioController.musicSources[4].volume = 0;
        }

        lives = 3;
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
