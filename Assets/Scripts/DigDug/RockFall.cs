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

        if (FALL)
            Debug.Log("HEYEADS");
    }

    private IEnumerator FALLING()
    {
        yield return null;
    }

    private void MOVEDOWN()
    {
        transform.position += Vector3.back * 0.2f;
    }
}
