using UnityEngine;

public class SpiderSpawn : MonoBehaviour
{
    [SerializeField]
    private float SpawnOpprotunity = 0f;
    [SerializeField]
    private GameObject SpiderPrefab;
    [SerializeField]
    private float SpiderSpawnChance = 0.3f;
    [SerializeField]
    private float SpawnInterval = 5f;
    [SerializeField]
    private Vector3 spawnPos;

    private void Update()
    {
        if (!MushroomScoreMarch.regenerating)
            SpawnOpprotunity += Time.deltaTime;
        if (SpawnOpprotunity >= SpawnInterval)
        {
            if (Random.Range(0f, 1f) <= SpiderSpawnChance)
                SpawnSpider();
            SpawnOpprotunity = 0f;
        }
    }

    private void SpawnSpider()
    {
        Debug.Log("Attempting to spawn spider...");
        if (FindAnyObjectByType<Spider>())
            return;
        Instantiate(SpiderPrefab, spawnPos, Quaternion.identity);
    }
}
