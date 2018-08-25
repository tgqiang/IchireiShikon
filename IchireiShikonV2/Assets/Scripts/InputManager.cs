using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    const string LAYER_NAME_TILE = "Tile";
    const float MOUSE_TAP_INPUT_TIME = 0.15f;

    float timePassedSinceButtonClick;

    [SerializeField]
    GameObject highlighter;

    Tile selectedTile;
    Tile currentTile;
    Mergeable selectedObject;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            OnClick();
        }

        if (Input.GetMouseButton(0)) {
            OnDrag();
        }

        if (Input.GetMouseButtonUp(0)) {
            OnRelease();
        }
	}

    void OnClick() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask(LAYER_NAME_TILE));

        if (hit.collider != null) {
            selectedTile = hit.collider.gameObject.GetComponent<Tile>();
            selectedObject = selectedTile.objectOnTile;
            if (selectedObject != null) {
                if (selectedObject is Spirit) {
                    (selectedObject as Spirit).ShowAreaOfEffect();
                }
            }
        }
    }

    void OnDrag() {
        if (selectedObject != null) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
                LayerMask.GetMask(LAYER_NAME_TILE));

            if (hit.collider != null) {
                currentTile = hit.collider.gameObject.GetComponent<Tile>();
                if (currentTile.IsVacant(selectedObject)) {
                    highlighter.transform.position = currentTile.transform.position;
                    highlighter.SetActive(true);
                    selectedObject.gameObject.transform.position = currentTile.transform.position;
                } else {
                    highlighter.SetActive(false);
                }
            }
        }

        timePassedSinceButtonClick += Time.deltaTime;
    }

    void OnRelease() {
        if (selectedObject != null) {
            highlighter.SetActive(false);
            if (selectedObject is Spirit) {
                (selectedObject as Spirit).HideAreaOfEffect();
            }

            bool wasDisplacedFromOriginalLocation = false;

            if (currentTile != null && currentTile.IsVacant(selectedObject)) {
                if (selectedTile != currentTile) wasDisplacedFromOriginalLocation = true;
                Tile.MoveToTile(selectedObject, selectedTile, currentTile);
            }

            if (timePassedSinceButtonClick <= MOUSE_TAP_INPUT_TIME) {
                if (selectedObject is Spirit) {
                    (selectedObject as Spirit).TriggerEffect();
                }
            } else {
                if (wasDisplacedFromOriginalLocation) {
                    if (selectedObject is Soul) (selectedObject as Soul).Merge();
                    else if (selectedObject is Spirit) (selectedObject as Spirit).Merge();
                }
            }
            
            selectedObject = null;
        }

        timePassedSinceButtonClick = 0;
    }
}
