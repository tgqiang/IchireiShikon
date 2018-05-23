using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTaint : MonoBehaviour {

    Tile parent;
    BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<Tile>();
        boxCollider = GetComponent<BoxCollider2D>();
	}

    void Update () {
        boxCollider.enabled = parent.IsTainted;
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (parent.IsTainted) {
            if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SOUL_BOUNDS) ||
                other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT_BOUNDS)) {
                other.gameObject.SetActive(false);
            }
        }
    }
}
