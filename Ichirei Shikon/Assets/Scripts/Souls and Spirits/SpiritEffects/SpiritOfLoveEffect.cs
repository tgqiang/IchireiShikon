using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfLoveEffect : MonoBehaviour {

    static int effectLevel;

    public SpiritLove owner;

    [SerializeField]
    GameObject[] zones;

    public bool[] isZoneOccupied;


	// Use this for initialization
	void Awake () {
        Debug.Assert(zones != null, "Array of effect zones not initialized for SpiritOfLoveEffect.");
        Debug.Assert(zones.Length == Configurable.instance.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[Configurable.instance.MAX_SPIRIT_LEVEL_BUFFED - 1], "Incorrect length of array of effect zones for SpiritOfLoveEffect.");

        Debug.Assert(isZoneOccupied != null, "Effect zone vacancy-tracking array not initialized for SpiritOfLoveEffect.");
        Debug.Assert(isZoneOccupied.Length == Configurable.instance.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[Configurable.instance.MAX_SPIRIT_LEVEL_BUFFED - 1], "Incorrect length of array of effect zone vacancy-tracking for SpiritOfLoveEffect.");
    }

    public void Highlight (SpiritLove caller, Vector3 targetPosition, int level) {
        owner = caller;

        transform.position = targetPosition;
        effectLevel = level;

        for (int i = 0; i < Configurable.instance.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[effectLevel - 1]; i++) {
            zones[i].SetActive(true);
        }
    }

    public void Unhighlight () {
        foreach (GameObject z in zones) {
            if (z.activeSelf) {
                z.SetActive(false);
            }
        }
    }

    public void CastEffect () {
        for (int i = 0; i < Configurable.instance.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[effectLevel - 1]; i++) {
            /*
             * A soul can only be spawned at vacant tiles.
             * If there is something occupying the tile where the soul could be spawned, the spawning
             * effect will NOT trigger.
             */
            if (zones[i].activeSelf && !isZoneOccupied[i]) {
                Configurable.instance.soulPool.SpawnRandomSoul(zones[i].transform.position);
            }
        }
    }
}
