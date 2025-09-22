using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HSHandler : MonoBehaviour
{
    private List<HighscoreElement> hsList = new List<HighscoreElement>(10);
    
    [SerializeField]
    private int MaxCount = 10;
    [SerializeField]
    private int Count;

    public string filename;

    private string Username;
    private bool NameDone;

    [SerializeField]
    private GameObject InputArea;
    private TextMeshProUGUI[] InputName = new TextMeshProUGUI[3];

    ScoreList sl;

    private void Start()
    {
        sl = gameObject.GetComponent<ScoreList>();
        LoadHS();

        InputName = InputArea.GetComponentsInChildren<TextMeshProUGUI>();

        InputArea.SetActive(false);
    }

    private void Update()
    {
        if (InputArea.activeSelf)
        {
            bool Change = false;

            InputName[Count].fontStyle = FontStyles.Underline;

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Change = true;
                Count--;

                if (Count == -1)
                    Count = 2;
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Change = true;
                Count++;

                if (Count == 3)
                    Count = 0;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                InputName[Count].GetComponent<NameInput>().PreviousLetter();
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                InputName[Count].GetComponent<NameInput>().NextLetter();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (TextMeshProUGUI tm in InputName)
                    Username = string.Join(" ", tm.text);

                NameDone = true;
            }

            if(Change)
            {
                foreach(TextMeshProUGUI tm in InputName)
                {
                    tm.fontStyle = FontStyles.Normal;
                }
            }
        }
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
                InputArea.SetActive(true);

                if (NameDone)
                {
                    NameDone = false;

                    HighscoreElement hs = AddHS(score);

                    hsList.Insert(i, hs);

                    while (hsList.Count > MaxCount)
                    {
                        hsList.RemoveAt(MaxCount);
                    }

                    SaveHS();

                    sl.UpdateUI(hsList);

                    InputArea.SetActive(false);

                    break;
                }
            }
        }
    }

    private HighscoreElement AddHS (int score)
    {
        HighscoreElement hse = new HighscoreElement(Username,score);

        return hse;
    }
}
