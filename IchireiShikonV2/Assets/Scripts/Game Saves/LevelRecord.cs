using System;
using UnityEngine;

public class LevelRecord : MonoBehaviour {

    public static LevelRecord instance;

    const string SAVE_FILE_PATH = "SaveFiles/Progress.txt";
    const char DELIMITER = ',';

    /// <summary>
    /// The maximum number of levels in each chapter.
    /// 
    /// Seeing as this may vary due to level design, this attribute is arbitrarily declared here as a temporary implementation measure.
    /// </summary>
    public static readonly int[] MAX_LEVELS = { 3, 0, 0, 0 };

    static string newLevelProgressData = "1";

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
        if (level == MAX_LEVELS[chapter - 1]) {
            if (chapter < MAX_CHAPTERS) {
                levelProgress[chapter] += 1;
            }
        } else {
            if (level == levelProgress[chapter - 1]) {
                levelProgress[chapter - 1] += 1;
            }
        }

        /* DEBUGGING */
        /* 
        foreach (int levels in levelProgress) {
            Debug.Log(levels);
        }
        */

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
        string records;

        if (ProjectConfig.BUILD_MODE) {
            records = SAVE_FILE_PATH.LoadFromPeristantDataPath_AsString(null);
        } else {
            records = SAVE_FILE_PATH.LoadFromDataPath_AsString(null);
        }

        if (records.IsNullOrEmpty()) {
            levelProgress = Array.ConvertAll<string, int>(newLevelProgressData.Split(DELIMITER), int.Parse);
            SaveLevelRecordToFile();
        } else {
            levelProgress = Array.ConvertAll<string, int>(records.Split(DELIMITER), int.Parse);
            VerifyLevelRecordIntegrity();
        }

        return GetLevelProgress();
    }

    public static void SaveLevelRecordToFile() {
        VerifyLevelRecordIntegrity();

        string data = "";
        for (int i = 0; i < levelProgress.Length; i++) {
            data += levelProgress[i];
            if (i < levelProgress.Length - 1) data.AppendComma();
        }

        if (ProjectConfig.BUILD_MODE) {
            // NOTE: for some reason, data.SaveToPersistentDataPath(null, SAVE_FILE_PATH);
            // writes into a file named {data} with content {Application.persistentDataPath + SAVE_FILE_PATH}
            //
            // This utility function from 'Extension Methods in Unity' package is probably broken.
            (Application.persistentDataPath + "/" + SAVE_FILE_PATH).SaveTo(data);
        } else {
            // NOTE: for the above case's reason, we also do away with calling
            // data.SaveToDataPath(null, SAVE_FILE_PATH); in 'Extension Methods in Unity' package.
            //
            // Am not entirely sure if data.SaveToDataPath(null, SAVE_FILE_PATH); works in Editor mode
            // but for safety precautions I would like to avoid weird/unexpected behaviours.
            (Application.dataPath + "/" + SAVE_FILE_PATH).SaveTo(data);
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

    /// <summary>
    /// This handles level-records when the game was closed via clicking on the 'X' button of the game window.
    /// </summary>
    void OnApplicationQuit() {
        SaveLevelRecordToFile();
    }
}
