using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfLoveEffectZone : MonoBehaviour {

    static SpiritOfLoveEffect parent;
    static Color effectiveColor = Color.white;
    static Color ineffectiveColor = new Color(1, 1, 1, 0.5f);

    [SerializeField]
    int zoneIndex;

    SpriteRenderer spriteRenderer;


	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "Missing SpriteRenderer component in SpiritOfLoveEfectZone game object.");

        parent = GetComponent<SpiritOfLoveEffect>();
        Debug.Assert(parent != null, "Missing SpiritOfLoveEffect parent component in SpiritOfLoveEffectZone game object.");

        Debug.Assert(zoneIndex >= 0 && zoneIndex < Constants.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[Constants.MAX_SPIRIT_LEVEL_BUFFED], "Incorrect zone index for SpiritOfLoveEffectZone.");
	}

    void OnTriggerEnter2D (Collider2D other) {
        if ((Equals(other.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_SOUL_BOUNDS)) ||
             Equals(other.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT_BOUNDS)))) {
            parent.isZoneOccupied[zoneIndex] = true;
            spriteRenderer.color = ineffectiveColor;
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if ((Equals(other.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_SOUL_BOUNDS)) ||
             Equals(other.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT_BOUNDS)))) {
            parent.isZoneOccupied[zoneIndex] = false;
            spriteRenderer.color = effectiveColor;
        }
    }
}
