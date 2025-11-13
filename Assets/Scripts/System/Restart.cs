using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public bool BO, CT;

    [SerializeField] //Test = Test Scenes | Multi = Cloned Player
    private bool Test, Multi;

    [SerializeField]
    private GameObject Player;
    private MoveInput mip; //mip = Move Input for Player
    private Coroutine Reseting, MultiReset; //Reseting = Reset Game After Death | MultiReset = Cloned Player Death
    private Vector3 OriPos, StopPos; //OriPos = Starting Position | //StopPos = End Position for Player
    private PlayerMove pInput;

    [SerializeField]
    private GameObject ReplayGo; //ReplayGo = Game Over Screen
    [SerializeField]
    private TextMeshProUGUI livetext; //livetext = Lives Counter Textbox
    public int lives = 3;
    private int totallives; //totallives = Lives Number to Reset to for Test Scenes

    public static event Action OnRestart; //OnRestart = Delegate to Restart different scripts

    [SerializeField]
    private AudioController AC;

    private bool Cheats;

    #region Breakout Variables
    private GameObject OriBall; //OriBall = Original Player Ball to take Variables from
    #endregion

    private void Start()
    {
        totallives = lives;

        mip = FindAnyObjectByType<MoveInput>();
        OriPos = mip.transform.position;

        Player = gameObject;

        PowerUp.OneUp += ExtraLife;

        if (ReplayGo != null)
            ReplayGo.SetActive(false);

        if (BO)
        {
            if (Multi)
                foreach (Restart RS in FindObjectsByType<Restart>(FindObjectsSortMode.None))
                    if (RS.Multi == false)
                        OriBall = RS.Player;
        }
    }

    private void OnEnable()
    {
        pInput = new PlayerMove();
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
        PowerUp.OneUp -= ExtraLife;
    }

    private void Update()
    {
        if (BO)
        {
            if (Multi)
            {
                lives = OriBall.GetComponent<Restart>().lives;
                Test = OriBall.GetComponent<Restart>().Test;
                livetext = OriBall.GetComponent<Restart>().livetext;
                ReplayGo = OriBall.GetComponent<Restart>().ReplayGo;
                AC = OriBall.GetComponent<Restart>().AC;
                Cheats = OriBall.GetComponent<Restart>().Cheats;
                Multi = false;
            }
        }

        livetext.text = lives.ToString();

        if (pInput.Movement.Play.IsPressed() && ReplayGo.activeSelf && Test)
            Replay();

        if (CheatCode.ACT)
            ACTCheat();
    }

    #region Breakout Reset
    public void BallReset()
    {
        if (FindFirstObjectByType<Camera>().GetComponent<Animator>())
            FindFirstObjectByType<Camera>().GetComponent<Animator>().SetTrigger("Shake");

        lives--;

        if (lives > 0)
            if (AC != null)
                AC.PlayVol(AC.floorSFX, 2f);

        StopPos = Player.transform.position;

            OnRestart();

        if (Reseting != null)
            StopCoroutine(Reseting);
        Reseting = StartCoroutine(ResetEnum());
    }

    public void MultiKill()
    {
        if (AC != null)
            AC.PlayVol(AC.floorSFX, 2f);

        StopPos = Player.transform.position;

        if (MultiReset != null)
            StopCoroutine(MultiReset);
        MultiReset = StartCoroutine(MultiEnum());
    }

    private IEnumerator MultiEnum()
    {
        Player.transform.position = StopPos;
        Player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);
        MultiReset = null;
        Destroy(this.gameObject);
    }
    #endregion

    public void CentiReset()
    {
        lives--;

        StopPos = Player.transform.position;

//        OnRestart();

        if (Reseting != null)
            StopCoroutine(Reseting);
        Reseting = StartCoroutine(ResetEnum());
    }

    public void GamaOvar()
    {
        if (AC != null)
        {
            AC.musicSource.Stop();
            AC.PlaySound(AC.gameOverSFX);
        }

        ReplayGo.SetActive(true);

            OnRestart();
    }

    private void Replay()
    {
        if (AC != null)
            AC.PlayVol(AC.nextLevelSFX, 5f);

        lives = totallives;
        ReplayGo.SetActive(false);
        mip.GameOver = false;
        mip.enabled = true;
        mip.gameObject.transform.position = OriPos;

        if (BO)
        {
            BlockRespawn BR = FindAnyObjectByType<BlockRespawn>();
            BR.TempRestart();
            Scoring SC = FindAnyObjectByType<Scoring>();
            SC.ResetScore();
        }

        StartAgain();
    }

    private void StartAgain()
    {
        if (BO)
            Player.GetComponent<BallMove>().Starto = true;
    }

    private IEnumerator ResetEnum()
    {
        if (BO)
        {
            Player.transform.position = StopPos;
            Player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

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

    private void ExtraLife()
    {
        lives++;
    }

    private void ACTCheat()
    {
        if (!Cheats)
        {
            Cheats = true;
            AC.PlayVol(AC.nextLevelSFX, 5f);
            lives += 100;
        }
    }
}
