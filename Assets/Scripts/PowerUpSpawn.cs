using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpPrefab;

    public void SpawnPowerUp()
    {
        float chance = Random.Range(0f, 100f);
        if (chance <= 20f)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
