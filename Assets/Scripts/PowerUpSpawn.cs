using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpPrefab;
    [SerializeField]
    private AudioSource powerupSFX;
    [SerializeField]
    private AudioClip powerupSpawn;

    public void SpawnPowerUp()
    {
        float chance = Random.Range(0f, 100f);
        if (chance <= 20f)
        {
            powerupSFX.PlayOneShot(powerupSpawn);
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
