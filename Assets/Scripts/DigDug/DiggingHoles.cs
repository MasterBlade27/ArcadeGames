using UnityEngine;

public class DiggingHoles : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer Dirt;
    private Collider Box;
    private RockFall Rocky;

    void Start()
    {
        Box = GetComponent<Collider>();
        Box.enabled = true;
        Dirt = GetComponent<MeshRenderer>();
        Dirt.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Rocky != null)
                Rocky.Dirts.Remove(this.gameObject);

            this.gameObject.SetActive(false);
//            Box.enabled = false;
  //          Dirt.enabled = false;
        }

        if (other.gameObject.GetComponentInParent<RockFall>() != null)
        {
            Rocky = other.gameObject.GetComponentInParent<RockFall>();

            if (other == Rocky.Checker)
                Rocky.Dirts.Add(this.gameObject);
        }
    }
}
