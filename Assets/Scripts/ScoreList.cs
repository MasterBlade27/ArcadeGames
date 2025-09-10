using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreList : MonoBehaviour
{
    [SerializeField]
    private GameObject ScoreListing;

    [SerializeField]
    private GameObject HSUIPrefab;
    [SerializeField]
    private Transform ElmWrapper;

    List<GameObject> UIElem = new List<GameObject>();

    void Start()
    {
        ScoreListing.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            ScoreListing.SetActive(true);
        else
            ScoreListing.SetActive(false);

    }

    public void UpdateUI(List<HighscoreElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            HighscoreElement el = list[i];

            if (el.Points > 0)
            {
                if (i >= UIElem.Count)
                {
                    var inst = Instantiate(HSUIPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(ElmWrapper.transform, false);

                    UIElem.Add(inst);

                }

                var texts = UIElem[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = el.Name;
                texts[1].text = el.Points.ToString();
            }
        }
    }
}
