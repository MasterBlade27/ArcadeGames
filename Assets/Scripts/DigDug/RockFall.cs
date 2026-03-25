using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class RockFall : MonoBehaviour
{
    public Collider Checker, RockSelf;
    public List<GameObject> Dirts = new List<GameObject>();
    [SerializeField]
    private bool FALL, STFall;

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
            STFall = true;
            RockSelf.enabled = true;
            StartCoroutine(FALLING());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Hey");
        if (!FALL && Dirts.Count >= 50)
        {
            Debug.Log("STOPPP");
            STFall = false;
            StopCoroutine(FALLING());
            StartCoroutine(BREAK());
        }
    }

    private IEnumerator FALLING()
    {
        while (STFall)
        {
            MOVEDOWN();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BREAK()
    {
        yield return new WaitForSeconds(0.9f);
        gameObject.SetActive(false);
    }

    private void MOVEDOWN()
    {
        transform.position += Vector3.back * 2f;
    }
}
