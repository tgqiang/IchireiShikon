using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfHarmonyEffectSpawnZone : MonoBehaviour {

    static SpiritOfHarmonyEffect parent;

    [SerializeField]
    int zoneIndex;


    void Awake () {
        parent = GetComponentInParent<SpiritOfHarmonyEffect>();
        Debug.Assert(parent != null, "Missing SpiritOfHarmonyEffect parent component in SpiritOfHarmonyEffectSpawnZone game object.");

        Debug.Assert(zoneIndex >= 0 && zoneIndex < Configurable.instance.SPIRIT_OF_HARMONY_SOULS_SPAWNED_AT_LEVEL[Configurable.instance.MAX_SPIRIT_LEVEL_BUFFED - 1], "Incorrect zone index for SpiritOfHarmonyEffectSpawnZone.");
    }

    void OnTriggerEnter2D (Collider2D other) {
        if ((Equals(other.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SOUL_BOUNDS])) ||
             Equals(other.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT_BOUNDS])))) {
            parent.isZoneOccupied[zoneIndex] = true;
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if ((Equals(other.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SOUL_BOUNDS])) ||
             Equals(other.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT_BOUNDS])))) {
            parent.isZoneOccupied[zoneIndex] = false;
        }
    }
}
