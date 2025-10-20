using UnityEngine;
using System.Collections.Generic;

public class LevelThingIGuess : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Levels = new List<GameObject>();

    private int levelCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCounter = -1;
        BlocksForLevels.nextLevel += NextLevel;

        NextLevel();
    }

    private void OnDisable()
    {
        BlocksForLevels.nextLevel -= NextLevel;
    }

    private void NextLevel()
    {
        levelCounter++;
        if (levelCounter >= Levels.Count)
            levelCounter = 0;

        var go = Instantiate(Levels[levelCounter], Vector3.zero, Quaternion.identity);
        go.transform.SetParent(transform, false);
    }
}
