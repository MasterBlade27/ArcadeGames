using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private HSHandler HSH;
    [SerializeField]
    private int totalscore;

    private PaddleMove pInput;

    private float Secs = 0.2f;

    [SerializeField]
    private Image Button;

    private void Start()
    {
        Button.color = Color.white;

        HSH = FindAnyObjectByType<HSHandler>();

        InputName = InputArea.GetComponentsInChildren<TextMeshProUGUI>();

        InParent.SetActive(false);
    }

    private void OnEnable()
    {
        pInput = new PaddleMove();
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
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

            if (pInput.Movement.Move.ReadValue<Vector2>().x < 0 && pInput.Movement.Move.IsPressed())
            {
                Change = true;
                Count--;

                if (Count == -1)
                    Count = 2;

                yield return new WaitForSeconds(Secs);
            }

            if (pInput.Movement.Move.ReadValue<Vector2>().x > 0 && pInput.Movement.Move.IsPressed())
            {
                Change = true;
                Count++;

                if (Count == 3)
                    Count = 0;

                yield return new WaitForSeconds(Secs);
            }

            if (pInput.Movement.Cycle.ReadValue<Vector2>().y > 0 && pInput.Movement.Cycle.IsPressed())
            {
                InputName[Count].GetComponent<NameArray>().PreviousLetter();

                yield return new WaitForSeconds(Secs);
            }

            if (pInput.Movement.Cycle.ReadValue<Vector2>().y < 0 && pInput.Movement.Cycle.IsPressed())
            {
                InputName[Count].GetComponent<NameArray>().NextLetter();

                yield return new WaitForSeconds(Secs);
            }

            if (pInput.Movement.Play.IsPressed())
            {
                Button.color = new Color(0f, 0.533f, 0f);

                Username = string.Join("", InputName[0].text, InputName[1].text, InputName[2].text);

                yield return new WaitForSeconds(Secs);

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

    public void NameSelect()
    {
        if (AddName != null)
            StopCoroutine(AddName);

        Username = string.Join("", InputName[0].text, InputName[1].text, InputName[2].text);

        InParent.SetActive(false);

        HSH.AddHS(Username, totalscore);

        if (GameOverScreen != null)
            StopCoroutine(GameOverScreen);
        GameOverScreen = StartCoroutine(GameOver());
    }
}
