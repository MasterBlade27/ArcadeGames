using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SpawnPowerUp()
    {
        float chance = Random.Range(0f, 100f);
        if (chance <= 20f)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
