using System.Collections.Generic;
using UnityEngine;

public class LevelTest : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> blocksTouching = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Fire();
    }
    private void OnEnable()
    {
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            //hit.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }

    private void Fire()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            hit.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }
}
