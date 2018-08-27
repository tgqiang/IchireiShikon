using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour {

    const float SCROLLING_DURATION = .3f;

    /// <summary>
    /// A list of game chapters.
    /// </summary>
    [SerializeField]
    GameObject[] chapters;
    [SerializeField]
    GameObject[] chapterNavButtons;

    /// <summary>
    /// A tracking variable used for handling level-selection chapter-scrolling. This is 1-indexed.
    /// 
    /// When the level-selection menu loads, the active chapter always defaults to the first chapter.
    /// </summary>
    int currentChapter = 1;

    /// <summary>
    /// A flag used for handling chapter-scrolling animations.
    /// This is to ensure that the animation completes nicely before the player can scroll again.
    /// </summary>
    bool isScrolling;

    void Start() {
        if (chapters.Length > 1) {
            foreach (GameObject b in chapterNavButtons) {
                b.SetActive(true);
            }
        }

        ShowUnlockedLevels();
    }

    public void ReturnToMainMenu() {
        SceneLoader.LoadMainMenu();
    }

    public void ShowUnlockedLevels() {
        // TODO: implement level-selection state initialization.
        // TODO: will also require file IO for reading/writing from level-completed file records.
    }

    public void MoveToPreviousChapter() {
        if (!isScrolling) {
            if (currentChapter > 1) {
                currentChapter--;
                StartCoroutine(TriggerScrollingAnimation(SCROLLING_DURATION));
            }
        }
    }

    public void MoveToNextChapter() {
        if (!isScrolling) {
            if (currentChapter <= chapters.Length) {
                currentChapter++;
                StartCoroutine(TriggerScrollingAnimation(SCROLLING_DURATION));
            }
        }
    }

    IEnumerator TriggerScrollingAnimation(float timeDelay) {
        isScrolling = true;
        // TODO: implement chapter-scrolling animation.
        /*
         * <insert code here>
         */
        yield return new WaitForSeconds(timeDelay);
        isScrolling = false;
    }

    public void LoadSelectedLevel(int levelNumber) {
        SceneLoader.LoadGameLevelScene(currentChapter, levelNumber);
    }
}
