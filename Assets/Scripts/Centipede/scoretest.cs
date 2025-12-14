using TMPro;
using UnityEngine;

public class scoretest : MonoBehaviour
{
    private int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText, endingScore;

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
