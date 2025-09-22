using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreName : MonoBehaviour
{
    [SerializeField]
    private int Count;
    private Coroutine AddName, GameOverScreen;

    [SerializeField]
    private string Username;

    [SerializeField]
    private GameObject InParent, InputArea;
    private TextMeshProUGUI[] InputName = new TextMeshProUGUI[3];

    [SerializeField]
    HSHandler HSH;
    [SerializeField]
    int totalscore;

    private void Start()
    {
        HSH = FindAnyObjectByType<HSHandler>();

        InputName = InputArea.GetComponentsInChildren<TextMeshProUGUI>();

        InParent.SetActive(false);
    }

    public void HighscoreRestart()
    {
        bool Checker;

        Scoring SC = FindAnyObjectByType<Scoring>();

        totalscore = SC.GiveScore();
        Checker = HSH.CheckHS(totalscore);

        if (Checker)
        {
            InParent.SetActive(true);

            if (AddName != null)
                StopCoroutine(AddName);
            AddName = StartCoroutine(NameInsert());
        }
        else
        {
            if (GameOverScreen != null)
                StopCoroutine(GameOverScreen);
            GameOverScreen = StartCoroutine(GameOver());
        }

    }

    private IEnumerator NameInsert()
    {
        while(InParent)
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
                InputName[Count].GetComponent<NameArray>().PreviousLetter();
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                InputName[Count].GetComponent<NameArray>().NextLetter();
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                Username = string.Join("", InputName[0].text, InputName[1].text, InputName[2].text);

                InParent.SetActive(false);

                HSH.AddHS(Username, totalscore);

                if (GameOverScreen != null)
                    StopCoroutine(GameOverScreen);
                GameOverScreen = StartCoroutine(GameOver());

                yield break;
            }

            if (Change)
            {
                foreach (TextMeshProUGUI tm in InputName)
                {
                    tm.fontStyle = FontStyles.Normal;
                }
            }
            yield return null;
        }
    }

    private IEnumerator GameOver()
    {
        Restart RS = FindAnyObjectByType<Restart>();
        RS.GamaOvar();

        yield return null;
    }
}
