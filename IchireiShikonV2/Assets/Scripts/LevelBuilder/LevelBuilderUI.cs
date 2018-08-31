using UnityEngine;
using UnityEngine.UI;

public class LevelBuilderUI : MonoBehaviour {

    const string EXPORT_FILE_SUBDIR = "/LevelData/";
    const string EXPORT_FILE_TYPE = ".csv";

    [SerializeField]
    GameObject levelExportPanel;
    [SerializeField]
    InputField filenameInputField;

    static LevelBuilderBackend levelBuilderBackend;

    void Start() {
        levelBuilderBackend = FindObjectOfType<LevelBuilderBackend>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.S)) {
            levelExportPanel.SetActive(true);
        }
	}

    public void CloseLevelExportPanel() {
        levelExportPanel.SetActive(false);
    }

    public void ExportLevel() {
        string filename = Application.streamingAssetsPath + EXPORT_FILE_SUBDIR + filenameInputField.text + EXPORT_FILE_TYPE;
        levelBuilderBackend.GetTilemapAsString().SaveTo(filename);
    }
}
