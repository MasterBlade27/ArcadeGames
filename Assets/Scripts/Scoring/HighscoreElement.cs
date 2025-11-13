using System;

[Serializable]
public class HighscoreElement
{
    public string Name;
    public int Points;

    public HighscoreElement (string name, int points)
    {
        Name = name;
        this.Points = points;
    }
}
