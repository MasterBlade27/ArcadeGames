using System.Collections;
using UnityEngine;

public class EnemyPump : MonoBehaviour
{
    [SerializeField]
    private int Inflate;
    [SerializeField]
    private float DeflationTime;
    [SerializeField] 
    private bool UpPump, PumpReady;
    public Coroutine CanPump;
    private PlayerMove pInput;
    private DrillShoot Enema;
    private DigMovement Domme;
    private Vector3 OriFlate;

    private void OnEnable()
    {
        PumpReady = false;

        OriFlate = transform.localScale;

        Enema = FindFirstObjectByType<DrillShoot>();
        Domme = FindFirstObjectByType<DigMovement>();

        DrillShoot.Pumping += PumpOn;

        pInput = new PlayerMove();
        pInput.Enable();
    }

    private void OnDisable()
    {
        DrillShoot.Pumping -= PumpOn;
        pInput.Disable();
    }

    private void Update()
    {
        if (Inflate >= 4)
        {
            Domme.PumpinAction = false;
            Enema.ResetPump();

            StopCoroutine(CanPump);
            CanPump = null;

            Inflate = 0;

            gameObject.SetActive(false);
        }

        transform.localScale = OriFlate + Vector3.one * Inflate * 2;
    }

    private void PumpOn()
    {
        if (CanPump != null)
            StopCoroutine(CanPump);
        CanPump = StartCoroutine(InflationPorn());
    }

    private IEnumerator InflationPorn()
    {
        Domme.PumpinAction = true;
        if (Inflate == 0)
            Inflate = 1;
        UpPump = false;
        DeflationTime = 1.5f;
        PumpReady = true;

        while (Inflate > 0)
        {
            DeflationTime -= 0.1f;

            if (pInput.Movement.Play.IsPressed() && PumpReady)
            {
                DeflationTime = 1.5f;

                UpPump = !UpPump;
                if (!UpPump)
                    Inflate++;

                yield return new WaitForSeconds(0.1f);
            }

            if (DeflationTime <= 0)
            {
                DeflationTime = 1.5f;

                Inflate--;
                if (Inflate == 0)
                    ResetEnemy();
            }

            if (pInput.Movement.FullMove.ReadValue<Vector2>().x != 0 ||
            pInput.Movement.FullMove.ReadValue<Vector2>().y != 0)
            {
                PumpReady = false;
                Enema.ResetPump();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ResetEnemy()
    {
        Domme.PumpinAction = false;
        Enema.ResetPump();

        StopCoroutine(CanPump);
        CanPump = null;
    }
}