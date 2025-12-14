using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushroomScoreMarch : MonoBehaviour
{
    public List<Mushroom> mushrooms = new List<Mushroom>();
    public static bool regenerating = false;
    private Coroutine m_Coroutine;

    void Start()
    {
       Player.gameReset += onReset;
    }
    private void OnDisable()
    {
        Player.gameReset -= onReset;
    }

    private void Update()
    {
        // optional: keep for debugging
        // Debug.Log(regenerating);
    }

    private void onReset()
    {
        // stop any running march and start a sequential one
        if (m_Coroutine != null)
            StopCoroutine(m_Coroutine);

        // collect mushrooms that need healing (use scene objects only)
        mushrooms.Clear();
        foreach (var m in Resources.FindObjectsOfTypeAll<Mushroom>())
        {
            if (m != null && m.damage > 0)
                mushrooms.Add(m);
        }

        m_Coroutine = StartCoroutine(SequentialScoreMarch());
    }

    private IEnumerator SequentialScoreMarch()
    {
        regenerating = true;

        for (int i = 0; i < mushrooms.Count; i++)
        {
            Mushroom m = mushrooms[i];
            if (m == null) 
                continue;

            // ensure the mushroom's done flag is reset before starting
            m.isDone = false;

            // start the mushroom's score-march / regen
            m.ScoreMarch();

            // wait until that single mushroom reports it finished
            yield return new WaitUntil(() => m.isDone);

            // optional short gap between mushrooms
            yield return null;
        }

        regenerating = false;
        m_Coroutine = null;
    }
}
