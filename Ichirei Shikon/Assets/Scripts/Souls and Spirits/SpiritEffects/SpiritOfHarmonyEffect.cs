using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfHarmonyEffect : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    static int effectLevel;

    [SerializeField]
    GameObject[] soulSpawnZones;
    public bool[] isZoneOccupied;

    List<Tile> tilesToPurify = new List<Tile>();
    List<Spirit> spiritsToBuff = new List<Spirit>();

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component missing from a game object containing the SpiritOfHarmonyEffect script.");

        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Assert(boxCollider != null, "BoxCollider2D component missing from a game object containing the SpiritOfHarmonyEffect script.");

        Debug.Assert(isZoneOccupied != null, "Effect zone vacancy-tracking array not initialized for SpiritOfHarmonyEffect.");
        Debug.Assert(isZoneOccupied.Length == Configurable.instance.SPIRIT_OF_HARMONY_SOULS_SPAWNED_AT_LEVEL[Configurable.instance.MAX_SPIRIT_LEVEL_BUFFED - 1], "Incorrect length of array of effect zone vacancy-tracking for SpiritOfHarmonyEffect.");
    }

    public void Highlight (Vector3 targetPosition, int level) {
        effectLevel = level;
        transform.position = targetPosition;
        Vector2 effectSize = (effectLevel * 2 + 1) * Vector2.one;
        boxCollider.size = effectSize - Configurable.instance.COLLIDER_SCALE_OFFSET * Vector2.one;
        spriteRenderer.size = effectSize;
        spriteRenderer.enabled = true;

        for (int i = 0; i < Configurable.instance.SPIRIT_OF_HARMONY_SOULS_SPAWNED_AT_LEVEL[effectLevel - 1]; i++) {
            soulSpawnZones[i].SetActive(true);
        }
    }

    public void Unhighlight () {
        spriteRenderer.enabled = false;

        foreach (GameObject z in soulSpawnZones) {
            if (z.activeSelf) {
                z.SetActive(false);
            }
        }
    }

    public void CastEffect () {
        foreach (Tile t in tilesToPurify) {
            t.Barrier();
        }

        foreach (Spirit s in spiritsToBuff) {
            if (s.Level <= effectLevel) {
                s.IncrementSpiritLevel(true);
            }
        }

        for (int i = 0; i < Configurable.instance.SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL[effectLevel - 1]; i++) {
            /*
             * A soul can only be spawned at vacant tiles.
             * If there is something occupying the tile where the soul could be spawned, the spawning
             * effect will NOT trigger.
             */
            if (soulSpawnZones[i].activeSelf && !isZoneOccupied[i]) {
                Configurable.instance.soulPool.SpawnRandomSoul(soulSpawnZones[i].transform.position);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponentInParent<Tile>();
        Spirit s = other.gameObject.GetComponentInParent<Spirit>();

        if (t != null && Equals(t.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]))) {
            if (!tilesToPurify.Contains(t)) {
                tilesToPurify.Add(t);
            }
        }

        if (s != null && Equals(s.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT_BOUNDS]))) {
            if (!spiritsToBuff.Contains(s)) {
                spiritsToBuff.Add(s);
            }
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        Tile t = other.gameObject.GetComponentInParent<Tile>();
        tilesToPurify.Remove(t);

        Spirit s = other.gameObject.GetComponentInParent<Spirit>();
        spiritsToBuff.Remove(s);
    }

}
