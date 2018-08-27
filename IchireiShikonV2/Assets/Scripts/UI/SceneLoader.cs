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

    public const int SCENE_INDEX_MAIN_MENU = 0;
    public const int SCENE_INDEX_LEVEL_SELECTION = 1;

    public static void LoadMainMenu() {
        SceneManager.LoadScene(SCENE_INDEX_MAIN_MENU);
    }

    public static void LoadLevelSelectionScene() {
        SceneManager.LoadScene(SCENE_INDEX_LEVEL_SELECTION);
    }

    public static void LoadGameLevelScene(int chapter, int levelNumber) {
        // TODO: implement game-level scene loading.
    }

    public static void LoadNextLevel(int currentLevel) {
        // TODO: implement next-level scene loading.
    }

    public static void ReloadCurrentScene(Scene currentScene) {
        SceneManager.LoadScene(currentScene.name);
    }

}
