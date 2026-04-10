using UnityEngine;

public class ScorpionSpawn : MonoBehaviour
{
    [SerializeField]
    private float SpawnOpprotunity = 0f;
    [SerializeField]
    private GameObject sPrefab;
    [SerializeField]
    private float sSpawnChance = 0.3f;
    [SerializeField]
    private float SpawnInterval = 5f;
    [SerializeField]
    private int SpawnLevel = 1;

    private Centipede Centipede;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Centipede = FindAnyObjectByType<Centipede>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MushroomScoreMarch.regenerating)
            SpawnOpprotunity += Time.deltaTime;
        if (SpawnOpprotunity >= SpawnInterval)
        {
            Scorpion scorpion = FindAnyObjectByType<Scorpion>();
            if (Random.Range(0f, 1f) <= sSpawnChance && Centipede.level >= SpawnLevel && scorpion == null)
                SpawnS();
            SpawnOpprotunity = 0f;
        }
    }

    private void SpawnS()
    {
        float spawnZ = Random.Range(-29f, 0f);
        spawnZ = Mathf.Round(spawnZ); // Align to grid
        Vector3 spawnPos = new Vector3(-11f, 0.5f, spawnZ);
        Instantiate(sPrefab, spawnPos, Quaternion.Euler(0f, -90f, 0f));
    }
}
