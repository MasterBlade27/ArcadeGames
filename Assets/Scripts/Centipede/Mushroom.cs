using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> damageLevels;
    private MeshFilter mushroomMfilter;
    private int health;
    private scoretest stest;

    [SerializeField]
    private AudioController AC;

    private void Awake()
    {
        stest = FindAnyObjectByType<scoretest>();
        mushroomMfilter = GetComponent<MeshFilter>();
        health = damageLevels.Count;
    }
    private void Start()
    {
        AC = FindAnyObjectByType<AudioController>();
        Player.gameReset += Restart;
    }
    private void OnDisable()
    {
        Player.gameReset -= Restart;
    }

    private void TakeDamage()
    {
        AC.PlaySound(AC.mushroomSFX);
        health--;
        if (health <= 0)
        {
            stest.scoreUpdate(5);
            Destroy(gameObject);
        }
        else
        {
            stest.scoreUpdate(1);
            mushroomMfilter.sharedMesh = damageLevels[damageLevels.Count - health - 1];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
    private void Restart()
    {
        health = damageLevels.Count;
        mushroomMfilter.sharedMesh = damageLevels[0];
    }
}
