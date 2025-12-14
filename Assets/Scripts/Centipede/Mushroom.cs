using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> damageLevels;
    public MeshFilter mushroomMfilter;
    public int damage = 0;
    private scoretest stest;

    [SerializeField]
    private AudioController AC;

    [SerializeField]
    private float regenStepDelay = 0.2f; // time between each health step during restart
    private Coroutine restartCorutine;
    public bool isDone = false;

    // static tracking for global regen state
    //public static int regeneratingCount = 0;
    //public static bool AnyHealing => regeneratingCount > 0;
    //public bool isRegeneratingInstance = false;

    private void Awake()
    {
        stest = FindAnyObjectByType<scoretest>();
        mushroomMfilter = GetComponent<MeshFilter>();
    }
    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        Player.mOnGameOver += onGameOver;
        //isRegeneratingInstance = false;
        mushroomMfilter.sharedMesh = damageLevels[0];
    }
    private void OnDisable()
    {
        Player.mOnGameOver -= onGameOver;
    }

    private void TakeDamage()
    {
        if (AC != null)
            AC.PlaySound(AC.mushroomSFX);

        damage++;
        if (damage >= damageLevels.Count)
        {
            stest.scoreUpdate(5);
            Destroy(gameObject);
        }
        else
        {
            stest.scoreUpdate(1);
            mushroomMfilter.sharedMesh = damageLevels[damage];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
    public void ScoreMarch()
    {
        Debug.Log("score march triggered");
        // Stop any running restart coroutine and start a new staggered regen only if not full health
        if (restartCorutine != null)
            StopCoroutine(restartCorutine);

        if (damage > 0)
        {
            /*if (!isRegeneratingInstance)
            {
                isRegeneratingInstance = true;
                regeneratingCount++;
            }*/
            restartCorutine = StartCoroutine(RestartCoroutine());
        }
    }

    private IEnumerator RestartCoroutine()
    { 
       isDone = false;
        while (damage > 0)
        {
            damage--;
            stest.scoreUpdate(5);

            if (AC != null)
                AC.PlaySound(AC.scoreMarchAudioClips[0]);

            if (damage == 0)
            {
                mushroomMfilter.sharedMesh = damageLevels[0];
            }
            else
            {
                mushroomMfilter.sharedMesh = damageLevels[damage];
            }

            yield return new WaitForSeconds(regenStepDelay);
        }
        isDone = true;

        restartCorutine = null;
    }
    private void onGameOver()
    {
        /*if (isRegeneratingInstance)
        {
            isRegeneratingInstance = false;
            regeneratingCount = 0;
        }*/
    }

    private IEnumerator RestartCorutine()
    {
        // kept for compatibility: unused
        yield break;
    }
}
