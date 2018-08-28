using System;
using UnityEngine;

public class LevelRecord : MonoBehaviour {

    public static LevelRecord instance;

    // Use these for build
    const string SAVE_DIR_BUILD = "SaveFiles";
    const string SAVE_FILENAME_BUILD = "Progress.txt";
    
    // Use this when working in Unity Editor
    const string SAVE_PATH_LOCAL = "Assets/Resources/SaveFiles/Progress.txt";

    /// <summary>
    /// The maximum number of levels in each chapter.
    /// 
    /// Seeing as this may vary due to level design, this attribute is arbitrarily declared here as a temporary implementation measure.
    /// </summary>
    public static readonly int[] MAX_LEVELS = { 3, 0, 0, 0 };

    /// <summary>
    /// The maximum number of chapters in the game.
    /// 
    /// Seeing as this may vary due to level design, this attribute is arbitrarily declared here as a temporary implementation measure.
    /// </summary>
    public const int MAX_CHAPTERS = 1;

    /// <summary>
    /// An array containing the number of levels, in each chapter, accessible by the player.
    /// </summary>
    /// 
    /// When the player first plays the game, the array is expected to look like { 1, 0, 0, ..., ... }
    /// since the first level is always accessible to the player.
    /// 
    /// Subsequently if the player finishes all, the array is expected to look like { MAX_LEVELS_CHPT_1, ..., ..., MAX_LEVELS_CHPT_N }.
    static int[] levelProgress;

    void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
            LoadLevelRecordFromFile();
        } else {
            Destroy(this);
        }
    }

    public static int[] GetLevelProgress() {
        return levelProgress;
    }

    /// <summary>
    /// Notifies the level-record that a specified chapter's level was cleared,
    /// which should make the next level in line accessible to the player.
    /// </summary>
    /// <param name="chapter">The cleared level's chapter, which is 1-indexed.</param>
    /// <param name="level">The level number of the cleared level, which is 1-indexed.</param>
    public static void UnlockNextLevelOnLevelClear(int chapter, int level) {
        if (chapter > MAX_CHAPTERS) {
            throw new System.Exception("Invalid chapter number detected when trying to unlock next level in level records. " +
                "Obtained chapter number [" + chapter + "] when maximum chapters is " + MAX_CHAPTERS + ".");
        }

        if (!level.IsBetweenInclusive(1, MAX_LEVELS[chapter - 1])) {
            throw new System.Exception("Invalid level number detected when trying to unlock next level in level records." +
                "Obtained level number [" + level + "] when maximum levels for chapter [" + chapter + "] is " + MAX_LEVELS[chapter - 1] + ".");
        }

        if (level == MAX_LEVELS[chapter - 1]) {
            if (chapter < MAX_CHAPTERS) {
                levelProgress[chapter] += 1;
            }
        } else {
            levelProgress[chapter - 1] += 1;
        }

        foreach (int levels in levelProgress) {
            Debug.Log(levels);
        }
        SaveLevelRecordToFile();
    }

    /// <summary>
    /// Checks if there is a next level available.
    /// </summary>
    /// <param name="currentChapter">The active level's chapter number, which is 1-based.</param>
    /// <param name="currentLevel">The active level's level number, which is 1-based.</param>
    /// <returns>True, if there is a next level, given the active level's chapter and level numbers; False otherwise.</returns>
    public static bool HasNextLevel(int currentChapter, int currentLevel) {
        if (currentLevel == MAX_LEVELS[currentChapter - 1]) {
            return MAX_LEVELS[currentChapter] > 0;
        } else {
            return currentLevel < MAX_LEVELS[currentChapter - 1];
        }
    }

    public static int[] LoadLevelRecordFromFile() {
        string[] records = null;

        if (ProjectConfig.BUILD_MODE) {
            records = SAVE_FILENAME_BUILD.LoadFromPeristantDataPath_AsString(SAVE_DIR_BUILD).Split(',');
        } else {
            records = SAVE_PATH_LOCAL.LoadFrom_AsString().Split(',');
        }

        levelProgress = Array.ConvertAll<string, int>(records, int.Parse);
        VerifyLevelRecordIntegrity();
        return GetLevelProgress();
    }

    public static void SaveLevelRecordToFile() {
        VerifyLevelRecordIntegrity();

        string data = "";
        for (int i = 0; i < levelProgress.Length; i++) {
            if (i < levelProgress.Length - 1) data.AppendComma();
        }

        if (ProjectConfig.BUILD_MODE) {
            SAVE_DIR_BUILD.SaveToPersistentDataPath(null, SAVE_FILENAME_BUILD);
        } else {
            data.SaveTo(SAVE_PATH_LOCAL);
        }
    }

    public static void VerifyLevelRecordIntegrity() {
        if (levelProgress.Length > MAX_CHAPTERS) {
            throw new System.Exception("Invalid number of game chapters detected when saving level progress. " +
                "Encountered obtained list containing [" + levelProgress.Length + "] chapters instead of the specified " + MAX_CHAPTERS + " chapters.");
        }

        for (int i = 0; i < levelProgress.Length; i++) {
            if (levelProgress[i] > MAX_LEVELS[i] || levelProgress[i] < 0) {
                throw new System.Exception("Invalid number of levels detected when reading level progress. " +
                "Encountered obtained levels in chapter [" + (i + 1) + "] containing " + levelProgress[i] +
                " levels instead of the specified " + MAX_LEVELS[i] + " levels.");
            }
        }
    }
}
