using UnityEngine;

public class ChapterLevels : MonoBehaviour {

    [SerializeField]
    GameObject[] levels;

    public void ShowAccessibleLevels(int accessibleLevelsCount) {
        for (int i = 0; i < accessibleLevelsCount; i++) {
            levels[i].SetActive(true);
        }
    }
}
