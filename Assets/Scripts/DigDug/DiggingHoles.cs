using UnityEngine;

public class DiggingHoles : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer Dirt;

    void Start()
    {
        Dirt = GetComponent<MeshRenderer>();
        Dirt.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HEYYYY");
        if (other.gameObject.CompareTag("Player")) 
            Dirt.enabled = false;
    }
}
