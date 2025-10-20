using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelThingIGuess : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Levels = new List<GameObject>();

    private int levelCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCounter = 0;
        BlocksForLevels.nextLevel += NextLevel;
        Instantiate(Levels[levelCounter]);
    }

    private void OnDisable()
    {
        BlocksForLevels.nextLevel -= NextLevel;
    }

    private void NextLevel()
    {
        levelCounter++;
        if (Levels.Count == levelCounter)
        {
            levelCounter = 0;
            Instantiate(Levels[levelCounter]);
        }
        else
            Instantiate(Levels[levelCounter]);
    }
}
