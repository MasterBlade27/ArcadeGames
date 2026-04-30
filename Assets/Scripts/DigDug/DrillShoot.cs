using System.Collections;
using System;
using UnityEngine;

public class DrillShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject Drill, Rope, Hand;
    [SerializeField]
    private int length;
    private Vector3 Dir, ExOriSca, ExOriPos, DrillOriPos;
    public Coroutine ExPu;

    public static event Action Pumping;

    private void Start()
    {
        ExOriSca = Rope.transform.localScale;
        ExOriPos = Rope.transform.localPosition;
        DrillOriPos = Drill.transform.localPosition;
    }

    public void Direction(float x, float y)
    {
        if (x > 0)
            Dir = new Vector3(2, 0, 0);
        else if (x < 0)
            Dir = new Vector3(-2, 0, 0);
        else if (y > 0)
            Dir = new Vector3(0, 0, 2);
        else if (y < 0)
            Dir = new Vector3(0, 0, -2);
    }

    public void Extend()
    {
        if (ExPu != null)
            StopCoroutine(ExPu);
        ExPu = StartCoroutine(ExtendoPumpo());
    }

    private IEnumerator ExtendoPumpo()
    {
        if (Dir == Vector3.zero)
            Dir = new Vector3(2, 0, 0);

        while (length < 20)
        {
            length++;
            Drill.transform.position += Dir;
            Rope.transform.localScale = new Vector3(0.1f, 0.1f * length, 0.1f);
            Rope.transform.position = (Hand.transform.position + Drill.transform.position) / 2;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        ResetPump();

        StopCoroutine(ExPu);
        ExPu = null;
    }

    public void ResetPump()
    {
        length = 0;
        Rope.transform.localScale = ExOriSca;
        Rope.transform.localPosition = ExOriPos;
        Drill.transform.localPosition = DrillOriPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Centipede"))
        {
            Pumping();
            if (ExPu != null)
            {
                StopCoroutine(ExPu);
                ExPu = null;
            }
        }

        if (other.CompareTag("Block"))
        {
            ResetPump();
            if (ExPu != null)
            {
                StopCoroutine(ExPu);
                ExPu = null;
            }
        }
    }
}
