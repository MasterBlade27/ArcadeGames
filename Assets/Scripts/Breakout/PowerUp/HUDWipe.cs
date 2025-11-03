using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDWipe : MonoBehaviour
{
    [SerializeField]
    private Image BG, Icon;
    private float MaxDura;
    private Coroutine Wipe;

    private void OnEnable()
    {
        Restart.OnRestart += EndWipe;
    }

    private void OnDisable()
    {
        Restart.OnRestart -= EndWipe;
    }

    public void Wiping(float Dura)
    {
        MaxDura = Dura;

        if (Wipe != null)
            StopCoroutine(Wipe);
        Wipe = StartCoroutine(TimedWipe());
    }

    private IEnumerator TimedWipe()
    {
        float timecounter = MaxDura;

        while (timecounter > 0)
        {
            timecounter -= Time.deltaTime;

            BG.fillAmount = timecounter / MaxDura;
            Icon.fillAmount = timecounter / MaxDura;
            yield return null;
        }

        if (timecounter < 0)
        {
            BG.gameObject.SetActive(false);
            yield break;
        }
    }

    private void EndWipe()
    {
        BG.gameObject.SetActive(false);
        StopCoroutine(Wipe);
    }
}
