using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for handling any scene-loading requests.
/// 
/// If any other classes in this game need to load scene(s), it should call the methods in <see cref="SceneLoader"/>
/// and not from <seealso cref="SceneManager"/>.
/// </summary>
public class SceneLoader : MonoBehaviour {

    const int SCENE_INDEX_MAIN_MENU = 0;
    const int SCENE_INDEX_LEVEL_SELECTION = 1;
    const int SCENE_INDEX_LEVEL_SCENE = 2;

    public static void LoadMainMenu() {
        SceneManager.LoadScene(SCENE_INDEX_MAIN_MENU);
    }

    public static void LoadLevelSelectionScene() {
        SceneManager.LoadScene(SCENE_INDEX_LEVEL_SELECTION);
    }

    /// <summary>
    /// Loads a specified game level.
    /// </summary>
    /// <param name="chapter">Chapter number of desired level to load, which is 1-based.</param>
    /// <param name="levelNumber">Level number of desired level to load, which is 1-based.</param>
    public static void LoadGameLevelScene(int chapter, int levelNumber) {
        Level.SetActiveLevel(chapter, levelNumber);
        SceneManager.LoadScene(SCENE_INDEX_LEVEL_SCENE);
    }

    /// <summary>
    /// Loads a level what is next-in-line to the current chapter's level.
    /// </summary>
    /// <param name="currentChapter">Chapter number of current level, which is 1-based.</param>
    /// <param name="currentLevel">Level number of current level, which is 1-based.</param>
    public static void LoadNextLevel(int currentChapter, int currentLevel) {
        if (currentLevel == LevelRecord.MAX_LEVELS[currentChapter - 1]) {
            if (currentChapter < LevelRecord.MAX_CHAPTERS) {
                LoadGameLevelScene(currentChapter + 1, 1);
            }
        } else {
            LoadGameLevelScene(currentChapter, currentLevel + 1);
        }
    }

    public static void ReloadCurrentScene(Scene currentScene) {
        SceneManager.LoadScene(currentScene.name);
    }

}
