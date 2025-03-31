using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance { get; private set; }

    public bool displayCompleted = false;
    public List<int> completedLevels = new();
    public int currentNodeIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }
    public void CompleteLevel(int levelId)
    {
        // na behandeling
        displayCompleted = true;
        if (!completedLevels.Contains(levelId))
        {
            completedLevels.Add(levelId);
        }
    }

    public void CompleteLevel(List<int> levelIds)
    {
        // inladen van scene
        foreach (int levelId in levelIds)
        {
            if (!completedLevels.Contains(levelId))
            {
                completedLevels.Add(levelId);
            }
        }
    }


}
