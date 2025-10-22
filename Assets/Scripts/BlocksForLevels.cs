using System;
using System.Collections.Generic;
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
            Debug.Log(blocks);
            blocks++;
        }
    }

    private void OnDisable()
    {
        ArmorBlock.blockDel -= theblockisdead;
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
