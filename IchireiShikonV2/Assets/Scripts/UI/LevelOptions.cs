using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This component handles all in-game UI functionality, including the game-over/victory screens.
/// 
/// This component should only be present in game level scenes and nowhere else.
/// </summary>
public class LevelOptions : MonoBehaviour {

    const int LEVEL_RESTART = 0;
    const int LEVEL_SELECT = 1;
    const int LEVEL_LEAVE_GAME = 2;

    [SerializeField]
    GameObject promptPanel;
    [SerializeField]
    Text promptText;

    int option;

    readonly string[] messages = { "Restart level?", "Go to Level Selection?", "Exit to Main Menu?" };

    /// <summary>
    /// Triggers the corresponding prompt when the in-game UI buttons are pressed.
    /// </summary>
    /// <param name="index"></param>
    public void OnSelect(int index) {
        FindObjectOfType<InputManager>().enabled = false;
        option = index;
        promptText.text = messages[index];
        promptPanel.SetActive(true);
    }

    /// <summary>
    /// When pressing an in-game UI button, a prompt for confirmation will appear.
    /// Pressing the 'Yes' button in the prompt window will proceed to execute the desired player action.
    /// 
    /// The desired player action corresponds to the 'index' argument passed into the <see cref="OnSelect(int)"/> function.
    /// </summary>
    public void Execute() {
        switch (option) {
            case LEVEL_RESTART:
                RestartCurrentLevel();
                break;
            case LEVEL_SELECT:
                ReturnToLevelSelection();
                break;
            case LEVEL_LEAVE_GAME:
                SceneLoader.LoadMainMenu();
                break;
            default:
                throw new System.Exception("Invalid index encountered.");
        }
    }

    /// <summary>
    /// This function is used for UI buttons shown on the game-over/victory screens.
    /// </summary>
    public void RestartCurrentLevel() {
        SceneLoader.ReloadCurrentScene(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// This function is used for UI buttons shown on the game-over/victory screens.
    /// </summary>
    public void ReturnToLevelSelection() {
        SceneLoader.LoadLevelSelectionScene();
    }

    /// <summary>
    /// This function is used for UI buttons shown on the game-over/victory screens.
    /// </summary>
    public void GoToNextLevel() {
        SceneLoader.LoadNextLevel(Level.chapter, Level.level);
    }

    public void ClosePrompt() {
        promptPanel.SetActive(false);
        FindObjectOfType<InputManager>().enabled = true;
    }
}
