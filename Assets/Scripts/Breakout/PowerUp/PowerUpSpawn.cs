using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private bool Demo;
    [SerializeField]
    private GameObject powerUpPrefab;
    [SerializeField]
    private AudioController AC;
    [SerializeField]
    private List<Material> powerUpIcons = new List<Material>();
    [SerializeField]
    private GameObject powerUpIconPrefab;
    private GameObject iconObj;
    private GameObject powerUp;
    private bool spawn = false;
    private int pUp;

    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        if(!Demo)
            TrySpawnIcon();
    }

    private void TrySpawnIcon()
    {
        float chance = Random.Range(0f, 100f);
        if (chance <= 30f)
        {
            if (powerUpIconPrefab != null && powerUpIcons.Count > 0)
            {
                spawn = true;
                iconObj = Instantiate(powerUpIconPrefab, transform.position, Quaternion.identity);
                iconObj.transform.SetParent(transform, false);

                iconObj.transform.localPosition = new Vector3(0f, 0f, -0.5f);
                iconObj.transform.localRotation = Quaternion.Euler(90f, 0f, 180f);

                pUp = Random.Range(0, powerUpIcons.Count);
                Material selectedIcon = powerUpIcons[pUp];
                Renderer iconRenderer = iconObj.GetComponent<Renderer>();
                if (iconRenderer != null)
                {
                    iconRenderer.material = selectedIcon;
                }

            }
        }
    }

    public void SpawnPowerUp()
    {
        if (spawn)
        {
            if(AC != null)
                AC.PlayVol(0.5f, AC.powerupSpawn, 0);
    
            powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            powerUp.GetComponent<PowerUp>().pUp = pUp;
            iconObj.SetActive(false);
        }
    }
}
