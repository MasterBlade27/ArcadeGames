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

    public void Appear(int index)
    {
        PupUI[index].SetActive(true);
        if (index < 6)
            PupUI[index].GetComponent<HUDWipe>().Wiping(5f);

        else if (index == 7)
            PupUI[index].GetComponent<HUDWipe>().Wiping(2.5f);

        else if (index == 6)
            MultiBall = true;
    }

    private void Update()
    {
        int balls = FindAnyObjectByType<BallMove>().CheckBalls();
        if (balls == 1)
        {
            MultiBall = false;
            PupUI[6].SetActive(false);
        }

    }
}
