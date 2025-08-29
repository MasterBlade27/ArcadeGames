using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class Scoring : MonoBehaviour
{
    [SerializeField]
    private Material mat;

    [SerializeField]
    private List<Material> MatList;
    [SerializeField]
    private List<Color> ColorList;
    [SerializeField]
    private List<int> score;

    int scoreCount = 0;

    [SerializeField]
    private TextMeshProUGUI scoretext, Hiscoretext;

    void Start()
    {
        for (int i = 0; i < MatList.Count; i++)
        {
            ColorList.Add(MatList[i].color);
            score.Add(i+3);
        }

        int highscore = PlayerPrefs.GetInt("highscore", scoreCount);
        string text = highscore.ToString();
        Hiscoretext.text = text;
    }

    private void Update()
    {
        scoretext.text = scoreCount.ToString();

        if (int.Parse(Hiscoretext.text) < scoreCount)
        {
            Hiscoretext.text = scoreCount.ToString();

            PlayerPrefs.SetInt("highscore", scoreCount);
        }
    }

    public void BlockScore(GameObject go)
    {
        mat = go.GetComponent<MeshRenderer>().material;

        for (int i = 0; i < score.Count; i++)
        {
            Debug.Log(mat);

            if (mat.color == ColorList[i])
                scoreCount += score[i];
        }
    }
}
