using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TileHighlighter : MonoBehaviour {

    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component is missing for a GameObject containing the TileHighlighter script.");

        Mergeable.tileHighlighter = this;
	}

    public void Highlight (Vector3 targetPosition) {
        transform.position = targetPosition;
        spriteRenderer.enabled = true;
    }

    public void Unhighlight () {
        spriteRenderer.enabled = false;
    }
}
