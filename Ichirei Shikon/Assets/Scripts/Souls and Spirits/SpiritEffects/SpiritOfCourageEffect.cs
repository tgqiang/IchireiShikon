using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritOfCourageEffect : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    List<Tile> tilesToPurify = new List<Tile>();
    List<Mergeable> itemsToDestroy = new List<Mergeable>();

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component missing from a game object containing the SpiritOfCourageEffect script.");

        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Assert(boxCollider != null, "BoxCollider2D component missing from a game object containing the SpiritOfCourageEffect script.");
    }

    public void Highlight (Vector3 targetPosition, int level) {
        transform.position = targetPosition;
        Vector2 effectSize = (level * 2 + 1) * Vector2.one;
        boxCollider.size = effectSize - Configurable.instance.COLLIDER_SCALE_OFFSET * Vector2.one;
        spriteRenderer.size = effectSize;
        spriteRenderer.enabled = true;
    }

    public void Unhighlight () {
        spriteRenderer.enabled = false;
    }

    public void CastEffect () {
        foreach (Mergeable m in itemsToDestroy) {
            m.gameObject.SetActive(false);
        }

        foreach (Tile t in tilesToPurify) {
            t.Purify();
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponent<Tile>();
        Mergeable m = other.gameObject.GetComponent<Mergeable>();

        if (t != null && Equals(t.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]))) {
            if (!tilesToPurify.Contains(t)) {
                tilesToPurify.Add(t);
            }
        }

        
        if (m != null) {
            if (!itemsToDestroy.Contains(m) &&
                (Equals(m.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SOUL_BOUNDS])) ||
                 Equals(m.gameObject.layer, LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT_BOUNDS])))) {
                itemsToDestroy.Add(m);
            }
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        Tile t = other.gameObject.GetComponent<Tile>();
        tilesToPurify.Remove(t);

        Mergeable m = other.gameObject.GetComponent<Mergeable>();
        itemsToDestroy.Remove(m);
    }

}
