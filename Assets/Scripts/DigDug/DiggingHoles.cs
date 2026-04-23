using UnityEngine;

public class DiggingHoles : MonoBehaviour
{
    [SerializeField]
    private GameObject VCorner, HCorner;
    [SerializeField]
    private bool CORN;
    private RockFall Rocky;

    private void Update()
    {
        if (CORN)
            if (!VCorner.activeSelf && !HCorner.activeSelf)
                this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Rocky != null)
                Rocky.Dirts.Remove(this.gameObject);

            this.gameObject.SetActive(false);
        }

        if (other.gameObject.GetComponentInParent<RockFall>() != null)
        {
            Rocky = other.gameObject.GetComponentInParent<RockFall>();

            if (other == Rocky.Checker)
                Rocky.Dirts.Add(this.gameObject);

            if (other == Rocky.RockSelf && Rocky.STFall)
            {
                Rocky.Dirts.Remove(this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }
}
