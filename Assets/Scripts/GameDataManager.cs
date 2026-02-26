using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int TotalScore = 0;
    public List<int> LevelDeaths = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int amount)
    {
        TotalScore += amount;
    }

    public void AddLevelDeaths(int deaths)
    {
        LevelDeaths.Add(deaths);
    }
}

