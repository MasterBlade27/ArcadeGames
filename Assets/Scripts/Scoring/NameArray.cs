using UnityEngine;
using TMPro;

public class NameArray : MonoBehaviour
{
    private char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    [SerializeField]
    private TextMeshProUGUI Display;

    private int LetterNumb = 0;

    public void NextLetter()
    {
        LetterNumb++;
        if (LetterNumb >= Alphabet.Length)
            LetterNumb = 0;

        UpdateLetterDisplay();
    }

    public void PreviousLetter()
    {
        LetterNumb--;
        if (LetterNumb < 0)
            LetterNumb = Alphabet.Length - 1;

        UpdateLetterDisplay();
    }

    private void OnEnable()
    {
        UpdateLetterDisplay();
    }

    private void UpdateLetterDisplay()
    {
        Display.text = Alphabet[LetterNumb].ToString();
    }
}