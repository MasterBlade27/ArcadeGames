using UnityEngine;

public class TickSpawn : MonoBehaviour
{
    [SerializeField]
    private float SpawnOpprotunity = 0f;
    [SerializeField]
    private GameObject TickPrefab;
    [SerializeField]
    private float TickSpawnChance = 0.3f;
    [SerializeField]
    private float SpawnInterval = 5f;
    [SerializeField]
    private float lBound, rBound;

    private void Update()
    {
        if(!MushroomScoreMarch.regenerating)
            SpawnOpprotunity += Time.deltaTime;
        if (SpawnOpprotunity >= SpawnInterval)
        {
            if(Random.Range(0f, 1f) <= TickSpawnChance)
                SpawnTick();
            SpawnOpprotunity = 0f;
        }
    }

    private void SpawnTick()
    {
        float spawnX = Random.Range(lBound, rBound);
        spawnX = Mathf.Round(spawnX); // Align to grid
        Vector3 spawnPos = new Vector3(spawnX, transform.position.y, transform.position.z);
        Instantiate(TickPrefab, spawnPos, Quaternion.identity);
    }
}
