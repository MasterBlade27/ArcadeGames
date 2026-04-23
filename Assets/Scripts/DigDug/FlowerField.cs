using System.Collections.Generic;
using UnityEngine;

public class FlowerField : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> FlowerList;
    [SerializeField, Range(1,278)]
    private int LvlCount;
    private int Tens, Ones;
    public bool LvlChange;

    private void Start()
    {
        foreach (var flower in FlowerList)
        {
            flower.transform.GetChild(0).gameObject.SetActive(false);
            flower.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (LvlChange)
        {
            LvlChange = false;

            UpdateFlowerField();
        }
    }

    private void UpdateFlowerField()
    {
        Tens = LvlCount / 10;
        Ones = LvlCount % 10;

        foreach (var flower in FlowerList)
        {
            flower.transform.GetChild(0).gameObject.SetActive(false);
            flower.transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < Tens; i++)
        {
            FlowerList[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = Tens; i < Tens + Ones; i++)
        {
            FlowerList[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
