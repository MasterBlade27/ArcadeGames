using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BlocksForLevels : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> itstheblocksorsometypeshit = new List<GameObject>();

    [SerializeField]
    private int blocks = 0;

    public static event Action nextLevel;

    void Start()
    {
        ArmorBlock.blockDel += theblockisdead;
        foreach (GameObject fuck in itstheblocksorsometypeshit)
        {
            fuck.SetActive(false);
            Debug.Log(blocks);
            blocks++;
        }
        spawn();
    }

    private void OnDisable()
    {
        ArmorBlock.blockDel -= theblockisdead;
    }

    private void spawn()
    {
        StartCoroutine(spawnRoutine());
    }
    private IEnumerator spawnRoutine()
    {
        // Wait until BallMove.wait is false without busy-waiting the main thread
        yield return new WaitUntil(() => !BallMove.wait);
        foreach (GameObject block in itstheblocksorsometypeshit)
        {
            yield return new WaitForSeconds(0.005f);
            block.SetActive(true);
        }

    }

    private void theblockisdead()
    {
        Debug.Log("block down");
        blocks--;
        if (blocks == 0 )
        {
            Debug.Log("end");
            nextLevel();
            Destroy(gameObject);
        }
    }
}
