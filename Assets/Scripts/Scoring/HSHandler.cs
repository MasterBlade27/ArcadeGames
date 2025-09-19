using System.Collections.Generic;
using UnityEngine;

public class HSHandler : MonoBehaviour
{
    private List<HighscoreElement> hsList = new List<HighscoreElement>(10);
    
    [SerializeField]
    private int MaxCount = 10;

    public string filename;

/*    [SerializeField]
    private GameObject InputArea = null;*/

    ScoreList sl;

    private void Start()
    {
        sl = gameObject.GetComponent<ScoreList>();
        LoadHS();
    }

    private void LoadHS()
    {
        hsList = FileHandler.ReadListFromJSON<HighscoreElement> (filename);

        while (hsList.Count > MaxCount)
        {
            hsList.RemoveAt (MaxCount);
        }

        if(hsList.Count != 0)
            sl.UpdateUI(hsList);
    }

    private void SaveHS()
    {
        FileHandler.SaveToJSON<HighscoreElement> (hsList, filename);
    }

    public void CheckHS(int score)
    {
        for (int i = 0; i < MaxCount; i++)
        {
            if (i >= hsList.Count || score > hsList[i].Points)
            {
                HighscoreElement hs = AddHS(score);

                hsList.Insert(i, hs);

                while (hsList.Count > MaxCount)
                {
                    hsList.RemoveAt(MaxCount);
                }

                SaveHS();

                sl.UpdateUI(hsList);

//                InputArea.SetActive(false);

                break;
            }
        }
    }

    private HighscoreElement AddHS (int score)
    {
//        InputArea.SetActive (true);

        string name;
        name = "ABC";

        HighscoreElement hse = new HighscoreElement(name,score);

        return hse;
    }
}
