using System.Collections.Generic;
using UnityEngine;

public class PowerUpHUD : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> PupUI = new List<GameObject>();

    private bool MultiBall = false;

    private void Start()
    {
        foreach (GameObject p in PupUI)
        {
            p.SetActive(false);
        }
    }

    public void Appear(int index, float Dura)
    {
        PupUI[index].SetActive(true);
        if (index < 6)
            PupUI[index].GetComponent<HUDWipe>().Wiping(Dura);

        else if (index == 7)
            PupUI[index].GetComponent<HUDWipe>().Wiping(2.5f);

        else if (index == 6)
            MultiBall = true;
    }

    private void Update()
    {
        if (MultiBall)
        {
            int balls = FindAnyObjectByType<BallMove>().CheckBalls();
            if (balls == 1)
            {
                MultiBall = false;
                PupUI[6].SetActive(false);
            }
        }
    }
}
