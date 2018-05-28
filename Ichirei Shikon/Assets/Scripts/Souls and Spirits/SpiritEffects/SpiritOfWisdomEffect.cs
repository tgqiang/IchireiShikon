using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfWisdomEffect : MonoBehaviour {

    public const int NUM_ZONES = 2;

    enum ZoneIndices {
        HORIZONTAL, VERTICAL
    }

    Vector2 horizontalSize = Vector2.one;
    Vector2 verticalSize = Vector2.one;

    [SerializeField]
    SpriteRenderer[] spriteRenderers;
    [SerializeField]
    BoxCollider2D[] boxColliders;

    public List<Tile> tilesToPurify = new List<Tile>();

    // Use this for initialization
    void Start () {
        Debug.Assert(spriteRenderers != null, "SpriteRenderer array missing from a game object containing the SpiritOfWisdomEffect script.");
        Debug.Assert(spriteRenderers.Length == NUM_ZONES, "SpriteRenderer array length for SpiritOfWisdomEffect script is incorrect.");

        Debug.Assert(boxColliders != null, "BoxCollider2D array missing from a game object containing the SpiritOfWisdomEffect script.");
        Debug.Assert(boxColliders.Length == NUM_ZONES, "BoxCollider2D array length for SpiritOfWisdomEffect script is incorrect.");
    }

    public void Highlight (Vector3 targetPosition, int level) {
        transform.position = targetPosition;

        horizontalSize.x = level * 2 - 1;
        verticalSize.y = level * 2 - 1;
        boxColliders[(int) ZoneIndices.HORIZONTAL].size = horizontalSize - Configurable.instance.COLLIDER_SCALE_OFFSET * Vector2.one;
        boxColliders[(int) ZoneIndices.VERTICAL].size = verticalSize - Configurable.instance.COLLIDER_SCALE_OFFSET * Vector2.one;

        spriteRenderers[(int) ZoneIndices.HORIZONTAL].size = horizontalSize;
        spriteRenderers[(int) ZoneIndices.VERTICAL].size = verticalSize;

        foreach (SpriteRenderer sr in spriteRenderers) {
            sr.enabled = true;
        }
    }

    public void Unhighlight () {
        foreach (SpriteRenderer sr in spriteRenderers) {
            sr.enabled = false;
        }
    }

    public void CastEffect () {
        foreach (Tile t in tilesToPurify) {
            t.Barrier();
        }
    }

}
