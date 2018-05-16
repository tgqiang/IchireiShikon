using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TileHighlighter : MonoBehaviour {

    public const string NAME = "TileHighlighter";
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component is missing for a GameObject containing the TileHighlighter script.");
	}

    public void Highlight (Vector3 targetPosition) {
        gameObject.transform.position = targetPosition;
        spriteRenderer.enabled = true;
    }

    public void Unhighlight () {
        Debug.Assert(spriteRenderer.enabled == true, "SpriteRenderer should not already be disabled before tile highlighting is stopped.");
        spriteRenderer.enabled = false;
    }
}
