using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpPrefab;
    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
    }

    public void SpawnPowerUp()
    {
        float chance = Random.Range(0f, 100f);
        if (chance <= 30f)
        {
            if(AC != null)
                AC.PlaySound(AC.powerupSpawn);
    
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
