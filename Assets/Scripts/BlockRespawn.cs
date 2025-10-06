using UnityEngine;

public class BlockRespawn : MonoBehaviour
{
    [SerializeField]
    private bool Test = true;

    [SerializeField]
    private GameObject BlockSpawn;

    [SerializeField]
    private GameObject[,] BlockArray;

    [SerializeField,Range(0f, 14f)]
    private int BlockCol;
    [SerializeField, Range(0f, 5f)]
    private int BlockRow;

    public int BlockActive;
    private int BlockClear = 0;

    [SerializeField]
    private AudioController AC;

    private int Level = 0;

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
                int cc = k * BlockCol + j;
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
        Level++;

        if (Test)
        {
            BlockClear = 0;
            AC.ResetMusic();
            AC.StartMusic(0);
        }

        if (AC != null)
        {
            AC.PlayVol(AC.oneUpSFX, 5f);

            for(int c = 0; c < Level; c++)
            {
                if (c < 2)
                {
                    AC.MusicLevel = c;
                    AC.StartMusic(c);
                }
            }
        }

        for (int k = 0; k < BlockRow; k++)
        {
            for (int j = 0; j < BlockCol; j++)
                BlockArray[k, j].SetActive(true);
        }

        BlockActive = BlockRow * BlockCol;
    }

    public void ResetLevels()
    {
        Level = 0;
    }

    public void CheckMusic()
    {
        if (BlockActive % 13 == 0)
        {
            BlockClear++;
            for (int i = 0; i < AC.musicSources.Count; i++)
            {
                if (BlockClear < 3)
                {
                    AC.PlaySound(AC.oneUpSFX);
                    AC.StartMusic(BlockClear);
                }
            }
        }
    }
}
