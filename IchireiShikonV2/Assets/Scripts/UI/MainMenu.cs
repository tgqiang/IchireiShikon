using UnityEngine;

/// <summary>
/// This component handles all main-menu UI functionality.
/// 
/// This component should be present in the main-menu scene and nowhere else.
/// </summary>
public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject creditsPanel;

    public void StartGame() {
        SceneLoader.LoadLevelSelectionScene();
    }

    public void ShowCredits() {
        creditsPanel.SetActive(true);
    }

    public void HideCredits() {
        creditsPanel.SetActive(false);
    }

    public void QuitGame() {
        LevelRecord.SaveLevelRecordToFile();
        Application.Quit();
    }
}
