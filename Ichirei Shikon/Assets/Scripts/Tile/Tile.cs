using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specification(s):
/// Every Tile object must be updated after each *move*.
/// (i.e. after player makes a move, every tile needs to be checked if
/// it will be tainted by adjacent tainted tiles, or if it will be purified)
/// </summary>
public class Tile : MonoBehaviour {

    const int UNTAINTED_TILE_SPRITE_INDEX = 0;
    const int SHIELDED_TILE_SPRITE_INDEX = 1;
    const int TAINTED_TILE_SPRITE_INDEX = 2;
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

    const int NUM_NEIGHBOUR_TILES = 4;
    /// <summary> Refers to the neighbouring left, right, top and bottom tiles of this tile. </summary> ///
    List<Tile> neighbours = new List<Tile>(NUM_NEIGHBOUR_TILES);


    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D (Collider2D other) {
        Tile t = other.gameObject.GetComponent<Tile>();
        if (t != null && !neighbours.Contains(t)) {
            neighbours.Add(t);
        }
    }

    public void InitializeTile (bool isTainted) {
        if (isTainted) {
            isTainted = true;
            spriteRenderer.sprite = tileSprites[TAINTED_TILE_SPRITE_INDEX];
        } else {
            spriteRenderer.sprite = tileSprites[UNTAINTED_TILE_SPRITE_INDEX];
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
                spriteRenderer.sprite = tileSprites[TAINTED_TILE_SPRITE_INDEX];
            }
        }
    }

    /// <summary>
    /// Turns this tile into an untainted one, if it was tainted previously.
    /// </summary>
    public void Purify () {
        if (isTainted) {
            tileManager.RemoveTileFromTaintedList(this);
            isTainted = false;
            spriteRenderer.sprite = tileSprites[UNTAINTED_TILE_SPRITE_INDEX];
        }
    }

    /// <summary>
    /// Shields this tile from tainting, if it wasn't shielded previously.
    /// </summary>
    public void Barrier () {
        if (!hasBarrier) {
            hasBarrier = true;
            spriteRenderer.sprite = tileSprites[SHIELDED_TILE_SPRITE_INDEX];
        }
    }

}
