using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> damageCap, damageStem;
    public MeshFilter CapMFilter, StemMFilter;
    public int damage = 0;
    private scoretest stest;

    [SerializeField]
    private AudioController AC;

    [SerializeField]
    private float regenStepDelay = 0.2f; // time between each health step during restart
    private Coroutine restartCorutine;
    public bool isDone = false;
    public bool Poison = false;
    private MeshRenderer cmr, smr;
    public Material pMat, goldmat;
    public Material OriCap, OriStem;

    // static tracking for global regen state
    //public static int regeneratingCount = 0;
    //public static bool AnyHealing => regeneratingCount > 0;
    //public bool isRegeneratingInstance = false;

    private void Awake()
    {
        stest = FindAnyObjectByType<scoretest>();
        CapMFilter = transform.GetChild(0).GetComponent<MeshFilter>();
        StemMFilter = transform.GetChild(1).GetComponent<MeshFilter>();
    }
    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        Player.mOnGameOver += onGameOver;
        cmr = transform.GetChild(0).GetComponent<MeshRenderer>();
        smr = transform.GetChild(1).GetComponent<MeshRenderer>();
        //isRegeneratingInstance = false;
        CapMFilter.sharedMesh = damageCap[0];
        StemMFilter.sharedMesh = damageStem[0];
    }
    private void OnDisable()
    {
        Player.mOnGameOver -= onGameOver;
    }

    private void Update()
    {

        if(Poison && cmr.material != pMat)
        {
            cmr.material = pMat;
            smr.material = pMat;
        }
    }

    private void TakeDamage()
    {
        if (AC != null)
            AC.PlayNormal(AC.mushroom, 6);

        damage++;
        if (damage >= damageCap.Count)
        {
            stest.scoreUpdate(50);
            Destroy(gameObject);
        }
        else
        {
            stest.scoreUpdate(10);
            CapMFilter.sharedMesh = damageCap[damage];
            StemMFilter.sharedMesh = damageStem[damage];
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
        cmr.material = goldmat;
        smr.material = goldmat;

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
                AC.PlayVol(1.5f, AC.scoreMarch, Random.Range(0, AC.scoreMarch.Count), 6);

            if (damage == 0)
            {
                CapMFilter.sharedMesh = damageCap[0];
                StemMFilter.sharedMesh = damageStem[0];
            }
            else
            {
                CapMFilter.sharedMesh = damageCap[damage];
                StemMFilter.sharedMesh = damageStem[damage];
            }

            yield return new WaitForSeconds(regenStepDelay);
        }
        isDone = true;
        cmr.material = OriCap;
        smr.material = OriStem;

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
