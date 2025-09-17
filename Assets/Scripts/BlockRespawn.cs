using UnityEngine;

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
        /*        BlockArray = new GameObject[BlockRow, BlockCol];
                for(int r = 0; r < BlockRow; r++)
                {
                    for(int c = 0; c < BlockCol; c++)
                        BlockArray[r, c] = BlockSpawn;
                }*/

        BlockActive = BlockRow * BlockCol;

        BlockArray = new GameObject[BlockRow, BlockCol];

        for (int k = 0; k < BlockRow; k++)
        {
            for (int j = 0; j < BlockCol; j++)
            {
                int cc = k * BlockRow + j;
                BlockArray[k, j] = transform.GetChild(cc).gameObject;
            }
        }    

//        TransBlock();
    }

    void Update()
    {
        if (BlockActive == 0)
            TempRestart();
//            RestartBlock();
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

    public void TempRestart()
    {
        for (int k = 0; k < BlockRow; k++)
        {
            for (int j = 0; j < BlockCol; j++)
                BlockArray[k, j].SetActive(true);
        }

        BlockActive = BlockRow * BlockCol;
    }
}
