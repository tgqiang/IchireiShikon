using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour {

    const float SCROLLING_DURATION = .3f;

    const int PREV = 0;
    const int NEXT = 1;
    const int FIRST = 0;
    int LAST;

    /// <summary>
    /// A list of game chapters.
    /// </summary>
    [SerializeField]
    GameObject[] chapters;
    [SerializeField]
    GameObject[] chapterNavButtons;

    /// <summary>
    /// A tracking variable used for handling level-selection chapter-scrolling. This is 0-indexed.
    /// 
    /// When the level-selection menu loads, the active chapter always defaults to the first chapter.
    /// </summary>
    int currentChapter = FIRST;

    /// <summary>
    /// A flag used for handling chapter-scrolling animations.
    /// This is to ensure that the animation completes nicely before the player can scroll again.
    /// </summary>
    bool isScrolling;

    void Start() {
        if (chapters.Length > 1) {
            chapterNavButtons[NEXT].SetActive(true);
            LAST = chapters.Length - 1;
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
            if (currentChapter > 0) {
                chapters[currentChapter].SetActive(false);
                currentChapter--;
                chapters[currentChapter].SetActive(true);
                chapterNavButtons[PREV].SetActive(currentChapter > FIRST);
                chapterNavButtons[NEXT].SetActive(currentChapter < LAST);
                StartCoroutine(TriggerScrollingAnimation(SCROLLING_DURATION));
            }
        }
    }

    public void MoveToNextChapter() {
        if (!isScrolling) {
            if (currentChapter < chapters.Length - 1) {
                chapters[currentChapter].SetActive(false);
                currentChapter++;
                chapters[currentChapter].SetActive(true);
                chapterNavButtons[PREV].SetActive(currentChapter > FIRST);
                chapterNavButtons[NEXT].SetActive(currentChapter < LAST);
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
