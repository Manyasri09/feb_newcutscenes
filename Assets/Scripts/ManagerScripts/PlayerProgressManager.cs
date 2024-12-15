using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerProgressManager
{
    // Keys for PlayerPrefs
    private const string TotalLevelsCompletedKey = "LevelsCompleted";
    private const string LevelTypeCompletedKeyPrefix = "LevelType_";
    private const string IndividualLevelKeyPrefix = "Level_";

    /// <summary>
    /// Call this method when a level is completed.
    /// </summary>
    /// <param name="levelID">The unique ID of the level.</param>
    /// <param name="levelType">The type of the level (e.g., "Easy", "Medium", "Hard").</param>
    public static void CompleteLevel(string levelID, string levelType)
    {
        Debug.Log("LevelID= " + levelID);
        Debug.Log("LevelType= " + levelType);
        // Track total levels completed
        int totalCompleted = PlayerPrefs.GetInt(TotalLevelsCompletedKey, 0);
        PlayerPrefs.SetInt(TotalLevelsCompletedKey, totalCompleted + 1);

        // Track level type completion
        string levelTypeKey = LevelTypeCompletedKeyPrefix + levelType;
        int levelTypeCompleted = PlayerPrefs.GetInt(levelTypeKey, 0);
        PlayerPrefs.SetInt(levelTypeKey, levelTypeCompleted + 1);

        // Mark individual level as completed
        string individualLevelKey = IndividualLevelKeyPrefix + levelID;
        PlayerPrefs.SetInt(individualLevelKey, 1);

        // Save changes
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Get the total number of levels completed.
    /// </summary>
    /// <returns>The total levels completed.</returns>
    public static int GetTotalLevelsCompleted()
    {
        return PlayerPrefs.GetInt(TotalLevelsCompletedKey, 0);
    }

    /// <summary>
    /// Get the number of completed levels of a specific type.
    /// </summary>
    /// <param name="levelType">The type of the level (e.g., "Easy", "Medium", "Hard").</param>
    /// <returns>The count of completed levels for the given type.</returns>
    public static int GetLevelTypeCount(string levelType)
    {
        string levelTypeKey = LevelTypeCompletedKeyPrefix + levelType;
        return PlayerPrefs.GetInt(levelTypeKey, 0);
    }

    /// <summary>
    /// Check if a specific level is completed.
    /// </summary>
    /// <param name="levelID">The unique ID of the level.</param>
    /// <returns>True if the level is completed, false otherwise.</returns>
    public static bool HasCompletedLevel(string levelID)
    {
        string individualLevelKey = IndividualLevelKeyPrefix + levelID;
        return PlayerPrefs.GetInt(individualLevelKey, 0) == 1;
    }

    /// <summary>
    /// Reset all progress (for testing or debugging purposes).
    /// </summary>
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
