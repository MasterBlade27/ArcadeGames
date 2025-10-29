using UnityEngine;
using System.Collections.Generic;

public class LevelThingIGuess : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Levels = new List<GameObject>();

    private int levelCounter;
    public static int levelSpeed = -1;

    [SerializeField]
    private AudioController AC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelSpeed = -1;
        levelCounter = -1;
        BlocksForLevels.nextLevel += NextLevel;

        NextLevel();
    }

    private void OnDisable()
    {
        BlocksForLevels.nextLevel -= NextLevel;
        BallMove.wait = false;
    }

    private void NextLevel()
    {
        if (AC != null)
            AC.PlaySound(AC.oneUpSFX);

        levelCounter++;
        levelSpeed++;
        Debug.Log(levelSpeed);
        if (levelCounter >= Levels.Count)
            levelCounter = 0;

        var go = Instantiate(Levels[levelCounter], Vector3.zero, Quaternion.identity);
        go.transform.SetParent(transform, false);
    }
}
