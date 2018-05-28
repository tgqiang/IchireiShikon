using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Specification(s):
/// Every Tile object must be updated after each *move*.
/// (i.e. after player makes a move, every tile needs to be checked if
/// it will be tainted by adjacent tainted tiles, or if it will be purified)
/// </summary>
public class Tile : MonoBehaviour {

    public enum SpriteIndices {
        UNTAINTED = 0, SHIELDED = 1, TAINTED = 2
    }
    /// <summary> Will be initialized by <see cref="TileManager"/> </summary> ///
    public static Sprite[] tileSprites;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    bool isTainted;
    public bool IsTainted {
        get { return isTainted; }
        set { isTainted = value; }
    }

    public static TileManager tileManager;

    /// <summary> Refers to whether it is shielded against demonic tainting or not. </summary> ///
    [SerializeField]
    bool hasBarrier;
    public bool HasBarrier {
        get { return hasBarrier; }
        set { hasBarrier = value; }
    }

    /// <summary> Refers to the neighbouring left, right, top and bottom tiles of this tile. </summary> ///
    List<Tile> neighbours = new List<Tile>(Configurable.NUM_NEIGHBOURS);


    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component is missing for a GameObject with Tile script.");
    }

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponent<Tile>();
        if (other.gameObject.layer == LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]) && !neighbours.Contains(t)) {
            neighbours.Add(t);
        }
    }

    public void InitializeTile (bool isTainted) {
        if (isTainted) {
            isTainted = true;
            UpdateSprite((int) SpriteIndices.TAINTED);
        } else {
            UpdateSprite((int) SpriteIndices.UNTAINTED);
        }
    }

    /// <summary>
    /// Spread the demonic tainting to neighbouring tiles, if this tile is tainted.
    /// </summary>
    /// <remarks>
    /// Only tainted tiles will call this function.
    /// </remarks>
    public void Spread () {
        foreach (Tile t in neighbours) {
            t.Taint();
        }
    }

    /// <summary>
    /// Turns this tile into a tainted one, if it isn't tainted previously. Note that a shielded tile cannot be tainted.
    /// </summary>
    public void Taint () {
        if (!hasBarrier) {
            if (!isTainted) {
                isTainted = true;
                UpdateSprite((int) SpriteIndices.TAINTED);
            }
        }
    }

    /// <summary>
    /// Turns this tile into an untainted one, if it was tainted previously.
    /// </summary>
    public void Purify () {
        if (isTainted) {
            Debug.Assert(tileManager != null, "'tileManager' is not initialized for GameObject with Tile script.");
            tileManager.RemoveTileFromTaintedList(this);
            isTainted = false;
            UpdateSprite((int) SpriteIndices.UNTAINTED);
        }
    }

    /// <summary>
    /// Shields this tile from tainting, if it wasn't shielded previously.
    /// </summary>
    public void Barrier () {
        if (isTainted) {
            tileManager.RemoveTileFromTaintedList(this);
            isTainted = false;
        }
        if (!hasBarrier) {
            hasBarrier = true;
            UpdateSprite((int) SpriteIndices.SHIELDED);
        }
    }

    void UpdateSprite (int index) {
        Debug.Assert(tileSprites != null, "'tileSprites' attribute is not initialized by TileManager.");
        Debug.Assert(tileSprites[index] != null, "tileSprite[" + index + "] is missing.");
        switch (index) {
            case (int) SpriteIndices.UNTAINTED:
                Debug.Assert(tileSprites[index].name == Configurable.instance.UNTAINTED_TILE_NAME, "tileSprites[" + index + "] does not match required sprite.");
                break;

            case (int) SpriteIndices.SHIELDED:
                Debug.Assert(tileSprites[index].name == Configurable.instance.SHIELDED_TILE_NAME, "tileSprites[" + index + "] does not match required sprite.");
                break;

            case (int) SpriteIndices.TAINTED:
                Debug.Assert(tileSprites[index].name == Configurable.instance.TAINTED_TILE_NAME, "tileSprites[" + index + "] does not match required sprite.");
                break;

            default:
                Debug.LogException(new System.Exception("Unexpected index encountered for UpdateSprite()."), this);
                break;
        }
        spriteRenderer.sprite = tileSprites[index];
    }

}
