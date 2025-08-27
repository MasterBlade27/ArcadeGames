using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlockRespawn : MonoBehaviour
{
    [SerializeField]
    private GameObject BlockSpawn;

    [SerializeField]
    private GameObject[,] BlockArray;

    [SerializeField,Range(0f, 100f)]
    private int BlockCol;
    [SerializeField, Range(0f, 5f)]
    private int BlockRow;

    public int BlockActive;

    private void Start()
    {
        //Row First, then Columns
        BlockArray = new GameObject[BlockRow, BlockCol];
        for(int r = 0; r < BlockRow; r++)
        {
            for(int c = 0; c < BlockCol; c++)
                BlockArray[r, c] = BlockSpawn;
        }

        BlockActive = BlockRow * BlockCol;

        TransBlock();
    }

    void Update()
    {
       if (BlockActive == 0)
            RestartBlock();
    }

    private void TransBlock()
    {

    }

    private void RestartBlock()
    {
        for (int r = 0; r < BlockRow; r++)
        {
            for (int c = 0; c < BlockCol; c++)
                BlockArray[r, c] = BlockSpawn;
        }

        BlockActive = BlockRow * BlockCol;
    }
}
