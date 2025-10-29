using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private bool Test, Multi;

    [SerializeField]
    private GameObject Ball, OriBall;
    private Vector3 StopPos;
    private Coroutine Reseting, MultiReset;

    public int lives = 3;
    private int totallives;

    [SerializeField]
    private TextMeshProUGUI livetext;

    [SerializeField]
    private GameObject ReplayGo;

    private MoveInput mip;
    private Vector3 OriPad;

    public static event Action OnRestart;

    [SerializeField]
    private AudioController AC;

    private PaddleMove pInput;

    private bool Cheats;

    private void Start()
    {
        totallives = lives;

        mip = FindAnyObjectByType<MoveInput>();
        OriPad = mip.transform.position;

        Ball = gameObject;

        PowerUp.OneUp += ExtraLife;

        if (ReplayGo != null)
            ReplayGo.SetActive(false);

        if (Multi)
            foreach (Restart RS in FindObjectsByType<Restart>(FindObjectsSortMode.None))
                if (RS.Multi == false)
                    OriBall = RS.Ball;
    }

    private void OnEnable()
    {
        pInput = new PaddleMove();
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
        PowerUp.OneUp -= ExtraLife;
    }

    private void Update()
    {
        if (Multi)
        {
            Test = OriBall.GetComponent<Restart>().Test;
            livetext = OriBall.GetComponent<Restart>().livetext;
            ReplayGo = OriBall.GetComponent<Restart>().ReplayGo;
            AC = OriBall.GetComponent<Restart>().AC;

            Multi = false;
        }

        livetext.text = lives.ToString();

        if (pInput.Movement.Play.IsPressed() && ReplayGo.activeSelf && Test)
            Replay();

        if (CheatCode.ACT)
            ACTCheat();
    }

    public void BallReset()
    {
        Debug.Log("Ball Reset");
        if (FindFirstObjectByType<Camera>().GetComponent<Animator>())
            FindFirstObjectByType<Camera>().GetComponent<Animator>().SetTrigger("Shake");

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

    public void GamaOvar()
    {
        if (AC != null)
            AC.musicSource.Stop();
            AC.PlaySound(AC.gameOverSFX);

        ReplayGo.SetActive(true);

            OnRestart();
    }

    private void Replay()
    {
        if (AC != null)
        {
            AC.PlayVol(AC.oneUpSFX, 5f);
        }

        lives = totallives;
        ReplayGo.SetActive(false);
        mip.GameOver = true;
        mip.enabled = true;
        mip.gameObject.transform.position = OriPad;

        BlockRespawn BR = FindAnyObjectByType<BlockRespawn>();
        BR.TempRestart();
        Scoring SC = FindAnyObjectByType<Scoring>();
        SC.ResetScore();

        StartAgain();
    }

    private void StartAgain()
    {
        Ball.GetComponent<BallMove>().Starto = true;
    }

    private IEnumerator ResetEnum()
    {
        Ball.transform.position = StopPos;
        Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        if (lives <= 0)
        {
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

    private void ExtraLife()
    {
        lives++;
    }

    private void ACTCheat()
    {
        if (!Cheats)
        {
            Cheats = true;
            AC.PlayVol(AC.oneUpSFX, 5f);
            lives += 100;
        }
    }
}
