using System.Collections.Generic;
using UnityEngine;

public class VegetableSpawn : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Vegetable;
    private int TotalCount, RockCount, Index;
    private bool Veg;
    public int Level;

    private void Start()
    {
        VegReset();
    }


    private void VegReset()
    {
        TotalCount = FindObjectsByType<RockFall>(FindObjectsSortMode.None).Length;
        Veg = false;
    }

    private void Update()
    {
        RockCount = FindObjectsByType<RockFall>(FindObjectsSortMode.None).Length;

        if (TotalCount - 2 == RockCount && !Veg)
        {
            Veg = true;
            SpawnVeg();
        }
    }

    private void SpawnVeg()
    {
//        Index = Level / 2;
//        Index = Mathf.Clamp(Index, 0, Vegetable.Count);


        Instantiate(Vegetable[0], transform.position, Quaternion.identity);
    }
}
