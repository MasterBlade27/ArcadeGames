using TMPro;
using UnityEngine;

public class scoretest : MonoBehaviour
{
    private int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText, Hiscoretext, endingScore;
    private HSHandler HSH;

    private void Start()
    {
        HSH = FindAnyObjectByType<HSHandler>();

        //Gather the Highscore from Stored Data
        int highscore = FileHandler.ReadTopFromJSON<HighscoreElement>(HSH.filename).Points;
        Debug.Log(highscore);
        //Updates the Highscore Text with the Stored Data
        Hiscoretext.text = highscore.ToString();
    }

    private void Update()
    {
        //Tests if the Highscore is lesser than the Score
        if (int.Parse(Hiscoretext.text) < score)
            //Updates the Highscore with the Score
            Hiscoretext.text = score.ToString();
    }

    public void scoreUpdate(int scoreAdd)
    {
        score += scoreAdd;
        scoreText.text = score.ToString();
    }

    public int GiveScore()
    {
        endingScore.text = score.ToString();

        return score;
    }
}
