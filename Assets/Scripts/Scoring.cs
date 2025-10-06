using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField]
    private List<Material> MatList;
    [SerializeField]
    private List<Color> ColorList;
    [SerializeField]
    private List<int> score;

    private int scoreCount = 0;

    HSHandler HSH;

    [SerializeField]
    private TextMeshProUGUI scoretext, Hiscoretext, EndingScore;

    private int multiplier = 1;

    void Start()
    {
        HSH = FindAnyObjectByType<HSHandler>();

        //Adding the Colors and Scores corresponding from the Materials
        for (int i = 0; i < MatList.Count; i++)
        {
            ColorList.Add(MatList[i].color);
            //Adds 3 on each color to differ the Score
            score.Add((i*10)+30);
        }

        PowerUp.ScoreMultiply += MutltiplyScore;
        Restart.OnRestart += onRestart;

        //Gather the Highscore from Stored Data
        int highscore = FileHandler.ReadTopFromJSON<HighscoreElement>(HSH.filename).Points;
        Debug.Log(highscore);
        //Updates the Highscore Text with the Stored Data
        Hiscoretext.text = highscore.ToString();
    }

    private void OnDisable()
    {
        PowerUp.ScoreMultiply -= MutltiplyScore;
        Restart.OnRestart -= onRestart;
    }

    private void Update()
    {
        //Updates the Score Text with Score Variable
        scoretext.text = scoreCount.ToString();
    
        if(EndingScore != null)
            EndingScore.text = scoreCount.ToString();

        //Tests if the Highscore is lesser than the Score
        if (int.Parse(Hiscoretext.text) < scoreCount)
        {
            //Updates the Highscore with the Score
            Hiscoretext.text = scoreCount.ToString();
            //Stores the new Highscore in Data

        }
    }

    //Custom Method used for giving Score for Blocks
    public void BlockScore(Material gomat)
    {
        for (int i = 0; i < score.Count; i++)
        {
            //See if the Block's Color is the same in the list
            if (gomat.color == ColorList[i])
                //Updates the Score with the corresponding Score
                scoreCount += score[i] * multiplier;
        }
    }

    public int GiveScore()
    {
        return scoreCount;
    }

    public void ResetScore()
    {
        scoreCount = 0;
    }

    private Coroutine multiplyCoroutine;

    private void MutltiplyScore(int mult, float duration)
    {
        if (multiplyCoroutine != null)
            StopCoroutine(multiplyCoroutine);
        multiplyCoroutine = StartCoroutine(Multiply(mult, duration));
    }

    private void onRestart()
    {
        if (multiplyCoroutine != null)
            StopCoroutine(multiplyCoroutine);
        multiplier = 1;
    }
    private IEnumerator Multiply(int mult, float duration)
    {
        multiplier = mult;
        yield return new WaitForSeconds(duration);
        multiplier = 1;
    }
}
