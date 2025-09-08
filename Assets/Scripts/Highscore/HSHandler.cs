using System.Collections.Generic;
using UnityEngine;

public class HSHandler : MonoBehaviour
{
    private List<HighscoreElement> hsList = new List<HighscoreElement>(10);
    
    [SerializeField]
    private int MaxCount = 10;

    [SerializeField]
    private string filename;

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

    public void AddHS (HighscoreElement hs)
    {
        for(int i = 0; i < MaxCount; i++)
        {
            if(i >= hsList.Count || hs.Points > hsList[i].Points)
            {
                Debug.Log(hsList[i] + " " + hs);
                hsList.Insert(i, hs);
                
                while (hsList.Count > MaxCount)
                {
                    hsList.RemoveAt(MaxCount);
                }

                SaveHS();

                sl.UpdateUI(hsList);

                break;
            }
        }
    }
}
