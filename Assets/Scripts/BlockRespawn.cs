using UnityEngine;

public class BlockRespawn : MonoBehaviour
{
    [SerializeField]
    private bool Test = true;

    private GameObject[] BlockArray;

    public int BlockActive;

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
                AC.PlaySound(AC.oneUpSFX);
            }

            foreach (var Block in BlockArray)
            {
                Block.SetActive(true);
            }

            BlockActive = BlockArray.Length;
        }
    }
}
