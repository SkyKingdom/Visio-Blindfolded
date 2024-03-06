using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : Manager
{
    public SeedManager()
    {
        if (gameManager.AutoGenerateSeed)
        {
            gameManager.Seed = Random.Range(0, int.MaxValue);
        }

        gameManager.LevelSeed = new System.Random(gameManager.Seed);
    }
}
