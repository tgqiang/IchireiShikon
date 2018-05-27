using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfHarmonyEffect : MonoBehaviour {

    public const string NAME = "SpiritHarmonyEffect";
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    List<Tile> tilesToPurify = new List<Tile>();
    List<Spirit> spiritsToBuff = new List<Spirit>();

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component missing from a game object containing the SpiritOfHarmonyEffect script.");

        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Assert(boxCollider != null, "BoxCollider2D component missing from a game object containing the SpiritOfHarmonyEffect script.");
    }

    public void Highlight (Vector3 targetPosition, int level) {
        transform.position = targetPosition;
        Vector2 effectSize = (level * 2 + 1) * Vector2.one;
        boxCollider.size = effectSize - Constants.COLLIDER_SCALE_OFFSET * Vector2.one;
        spriteRenderer.size = effectSize;
        spriteRenderer.enabled = true;
    }

    public void Unhighlight () {
        spriteRenderer.enabled = false;
    }

    public void CastEffect (int effectLevel) {
        foreach (Tile t in tilesToPurify) {
            t.Barrier();
        }

        foreach (Spirit s in spiritsToBuff) {
            if (s.Level <= effectLevel) {
                s.IncrementSpiritLevel(true);
            }
        }

        // TODO: Implement soul-spawning effect
    }

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponentInParent<Tile>();
        Spirit s = other.gameObject.GetComponentInParent<Spirit>();

        if (t != null && Equals(t.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_TILE))) {
            if (!tilesToPurify.Contains(t)) {
                tilesToPurify.Add(t);
            }
        }

        if (s != null && Equals(s.gameObject.layer, LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT_BOUNDS))) {
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
