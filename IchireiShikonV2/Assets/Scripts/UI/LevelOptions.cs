using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This component handles all in-game UI functionality.
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

    public void OnSelect(int index) {
        FindObjectOfType<InputManager>().enabled = false;
        option = index;
        promptText.text = messages[index];
        promptPanel.SetActive(true);
    }

    public void Execute() {
        switch (option) {
            case LEVEL_RESTART:
                SceneLoader.ReloadCurrentScene(SceneManager.GetActiveScene());
                break;
            case LEVEL_SELECT:
                SceneLoader.LoadLevelSelectionScene();
                break;
            case LEVEL_LEAVE_GAME:
                SceneLoader.LoadMainMenu();
                break;
            default:
                throw new System.Exception("Invalid index encountered.");
        }
    }

    public void ClosePrompt() {
        promptPanel.SetActive(false);
        FindObjectOfType<InputManager>().enabled = true;
    }
}
