using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfWisdomEffectZones : MonoBehaviour {

    static SpiritOfWisdomEffect parent;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<SpiritOfWisdomEffect>();
        Debug.Assert(parent != null, "Missing parent component in SpiritOfWisdomEffectZones script.");
	}

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponentInParent<Tile>();

        if (t != null && Equals(t.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]))) {
            if (!parent.tilesToPurify.Contains(t)) {
                parent.tilesToPurify.Add(t);
            }
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        Tile t = other.gameObject.GetComponentInParent<Tile>();
        parent.tilesToPurify.Remove(t);
    }
}
