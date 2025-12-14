using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool Cheats;

    public int lives = 3;
    [SerializeField]
    private TextMeshProUGUI livetext;
    [SerializeField]
    private GameObject GOS;

    private Centipede pCentipede => FindAnyObjectByType<Centipede>();

    private Vector3 startPosition;

    // Optional: assign in inspector; will fallback to GetComponent<Renderer>()
    [SerializeField]
    private Renderer playerRenderer;
    [SerializeField]
    private float blinkDuration = 1f;
    [SerializeField]
    private float blinkInterval = 0.1f;

    [SerializeField]
    private AudioController AC;

    private Coroutine blinkCoroutine;

    public static event Action gameReset;
    public static event Action mOnGameOver;
    private float killCooldown = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (livetext != null)
            livetext.text = lives.ToString();

        //Debug.Log(killCooldown);
        if (killCooldown < 0.5f)
            killCooldown += Time.deltaTime;

        if (CheatCode.ACT)
            ACTCheat();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected with " + collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Centipede"))
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        if (AC != null)
            AC.PlaySound(AC.playerHitSFX);
    
        if (killCooldown >= 0.5f)
        {
            killCooldown = 0f;
            lives--;
            transform.position = startPosition;
            playerBlink();
            pCentipede.Respawn();
            gameReset();
            if (lives <= 0)
            {
                mOnGameOver();

                GetComponent<MoveTest>().enabled = false;
                ScoreName SN = FindAnyObjectByType<ScoreName>();
                SN.HighscoreRestart();
            }
        }
    }

    private void playerBlink()
    {
        if (playerRenderer == null)
            playerRenderer = GetComponent<Renderer>();

        if (playerRenderer == null)
            return; // no renderer to blink

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < blinkDuration)
        {
            visible = !visible;
            playerRenderer.enabled = visible;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // Ensure renderer ends visible
        playerRenderer.enabled = true;
        blinkCoroutine = null;
    }

    public void GamaOvar()
    {
        if (AC != null)
        {
            AC.musicSource.Stop();
            AC.PlaySound(AC.gameOverSFX);
        }

        GOS.SetActive(true);
        Destroy(gameObject);
    }

    private void ACTCheat()
    {
        if (!Cheats)
        {
            Cheats = true;
            AC.PlayVol(AC.nextLevelSFX, 1.5f);
            lives += 100;
        }
    }
}