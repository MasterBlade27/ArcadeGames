using UnityEngine;

public class BlockRespawn : MonoBehaviour
{
    [SerializeField]
    private bool Test = true;

    private GameObject[] BlockArray;

    public int BlockActive;
    private int BlockClear = 0;

    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        if (Test)
        {
            var BlockArr = FindObjectsByType<ArmorBlock>(FindObjectsSortMode.None);
            BlockArray = new GameObject[BlockArr.Length];
            for (int i = 0; i < BlockArr.Length; i++)
                BlockArray[i] = BlockArr[i].gameObject;

            BlockActive = BlockArray.Length;
        }
    }

    void Update()
    {
        if (BlockActive == 0)
            TempRestart();
    }

    public void TempRestart()
    {
        if (Test)
        {
            if (AC != null)
            {
                BlockClear = 0;
                AC.ResetMusic();
                AC.StartMusic(0);
            }

            foreach (var Block in BlockArray)
            {
                Block.SetActive(true);
            }

            BlockActive = BlockArray.Length;
        }
    }

    public void CheckMusic()
    {
        if (AC != null)
        {
            if (BlockActive % BlockArray.Length / 3 == 0)
            {
                BlockClear++;
                if (BlockClear < 3)
                {
                    AC.PlaySound(AC.oneUpSFX);
                    AC.StartMusic(BlockClear);
                }
            }
        }
    }
}
