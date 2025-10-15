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
    private Coroutine Reseting, MultiReset;

    public int lives = 3;
    private int totallives;

    [SerializeField]
    private TextMeshProUGUI livetext;

    [SerializeField]
    private GameObject ReplayGo;

    private MoveInput mip;
    private MouseMobile mm;
    private Vector3 OriPad;

    public static event Action OnRestart;

    [SerializeField]
    private AudioController AC;

    private PaddleMove pInput;

    private void Start()
    {
        totallives = lives;

        mip = FindAnyObjectByType<MoveInput>();
        mm = FindAnyObjectByType<MouseMobile>();
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

        if(ReplayGo.activeSelf)
        {

        }
    }

    public void BallReset()
    {
        lives--;

        if (lives > 0)
            if (AC != null)
                AC.PlayVol(AC.floorSFX, 2f);

        StopPos = Ball.transform.position;

            OnRestart();

        if (Reseting != null)
            StopCoroutine(Reseting);
        Reseting = StartCoroutine(ResetEnum());
    }

    public void MultiKill()
    {
            if (AC != null)
                AC.PlayVol(AC.floorSFX, 2f);

        StopPos = Ball.transform.position;

        if (MultiReset != null)
            StopCoroutine(MultiReset);
        MultiReset = StartCoroutine(MultiEnum());
    }

    private void StartAgain()
    {
        Ball.GetComponent<BallMove>().Starto = true;
    }

    public void GamaOvar()
    {
        if (AC != null)
            AC.PlaySound(AC.gameOverSFX);

        ReplayGo.SetActive(true);

            OnRestart();
    }

    private void Replay()
    {
        if (AC != null)
        {
            AC.StartMusic(0);

            AC.PlayVol(AC.oneUpSFX, 5f);
        }

        lives = totallives;
        ReplayGo.SetActive(false);
        mip.GameOver = true;
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
        Ball.transform.position = StopPos;
        Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        if (lives <= 0)
        {
            if (AC != null)
                AC.ResetMusic();

            mip.GameOver = true;
            mip.enabled = false;

            if (Test)
            {
                GamaOvar();
                yield break;
            }

            ScoreName SN = FindAnyObjectByType<ScoreName>();
            SN.HighscoreRestart();

            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        StartAgain();
        Reseting = null;
    }

    private IEnumerator MultiEnum()
    {
        Ball.transform.position = StopPos;
        Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);
        MultiReset = null;
        Destroy(this.gameObject);
    }
}
