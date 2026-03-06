using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    public Collider Checker, RockSelf;
    public List<GameObject> Dirts = new List<GameObject>();
    [SerializeField]
    private bool FALL;

    private void OnEnable()
    {
        RockSelf.enabled = false;
    }

    private void Update()
    {
        if (Dirts.Count == 0)
            FALL = true;

        else
            FALL = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (FALL && other.CompareTag("Player"))
        {
            RockSelf.enabled = true;
            Checker.enabled = false;
            StartCoroutine(FALLING());
        }
    }

    private IEnumerator FALLING()
    {
        while (FALL)
        {
            MOVEDOWN();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void MOVEDOWN()
    {
        transform.position += Vector3.back * 2f;
    }
}
