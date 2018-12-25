using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public static Level gameLevel;

    /// <summary>
    /// The coordinates on the tile map in {row, column} notation, where 'x' denotes row and 'y' denotes column.
    /// 
    /// Row and column counting starts from top-left corner of the tile map.
    /// 
    /// Additionally, row = -Y, column = X if {X,Y} are the X-Y coordinates of the object's transform.
    /// </summary>
    public Vector2Int tileCoords;

    public bool isTainted;
    public bool isPurified;
    public bool isInvulnerable;

    /// <summary>
    /// This attribute checks for which <seealso cref="Mergeable"/> object is placed on this <see cref="Tile"/>.
    /// </summary>
    public Mergeable objectOnTile;

    public Sprite[] tileSprites;

    [SerializeField]
    ParticleSystem tileTaintParticleSystem;
    [SerializeField]
    ParticleSystem tilePurifyParticleSystem;

    void Start() {
        gameLevel = FindObjectOfType<Level>();
    }

    /// <summary>
    /// This loop ensures that any <seealso cref="Mergeable"/> object that lands on a tainted <see cref="Tile"/>
    /// becomes tainted.
    /// 
    /// See <seealso cref="Soul.Taint"/> and <seealso cref="Spirit.Taint"/> for the specified taint behaviours.
    /// </summary>
    void Update() {
        if (isTainted & !IsVacant()) {
            if (objectOnTile is Soul) (objectOnTile as Soul).Taint();
            else if (objectOnTile is Spirit) (objectOnTile as Spirit).Taint();
        }
    }

    /// <summary>
    /// Tests whether this tile is vacant.
    /// 
    /// If 'obj' argument is supplied, it also tests if the tile already contains 'obj'.
    /// If the tile already contains 'obj', that tile is deemed to be vacant.
    /// </summary>
    /// <param name="obj">The object, to test if it is already contained in a desired tile.</param>
    /// <returns>True, if this tile isn't holding anything, or if this tile already contains 'obj'; False otherwise.</returns>
    public bool IsVacant(Mergeable obj = null) {
        return objectOnTile == null || objectOnTile == obj;
    }

    /// <summary>
    /// Taints this <see cref="Tile"/>. This cannot affect invulnerable <see cref="Tile"/>s.
    /// 
    /// Returns a reference to itself for record-keeping at a later stage of the taint-spreading.
    /// This reference-return feature is required because, without differentiating newly-tainted tiles from
    /// pre-existing tainted tiles, this taint will spread infinitely and this should NOT be the case.
    /// </summary>
    /// /// <param name="fromEndOfTurn">Indicates whether this taint is performed from end-of-turn taint spreading or not.</param>
    /// <returns>Itself, if this tile was successfully tainted; Null if this tile cannot be tainted or if this tile is already tainted.</returns>
    public Tile Taint(bool fromEndOfTurn = false) {
        // TODO: add a fancier animation for tainting of tile.
        if (!isInvulnerable) {
            if (!isTainted) {
                isTainted = true;
                isPurified = false;
                PlayParticleEffect(CustomEnums.TileType.TAINTED);
                GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.TAINTED];

                if (objectOnTile != null) {
                    if (objectOnTile is Soul) {
                        RemoveFromTileAndDestroy(objectOnTile, this);
                    } else if (objectOnTile is Spirit) {
                        objectOnTile.Taint();
                    }
                }

                if (!fromEndOfTurn) {
                    if (!gameLevel.levelData.taintedTiles.Contains(this)) {
                        gameLevel.levelData.taintedTiles.Add(this);
                    }
                }
                
                return this;
            } else {
                return null;
            }
        } else {
            return null;
        }
    }

    /// <summary>
    /// Purifies this <see cref="Tile"/>, which removes the taint present on it.
    /// </summary>
    public void Purify() {
        // TODO: add a fancier animation for purification of tile.
        if (!isPurified || !isInvulnerable) {
            isTainted = false;
            isPurified = true;
            PlayParticleEffect(CustomEnums.TileType.PURIFIED);
            GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.PURIFIED];

            if (gameLevel.levelData.taintedTiles.Contains(this)) {
                gameLevel.levelData.taintedTiles.Remove(this);
            }
        }
    }

    /// <summary>
    /// Purifies this <see cref="Tile"/> and renders it invulnerable to <see cref="Taint"/> with a barrier.
    /// </summary>
    public void PurifyWithBarrier() {
        if (!isInvulnerable) {
            isTainted = false;
            isPurified = true;
            isInvulnerable = true;
            PlayParticleEffect(CustomEnums.TileType.INVULNERABLE);
            GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.INVULNERABLE];

            if (gameLevel.levelData.taintedTiles.Contains(this)) {
                gameLevel.levelData.taintedTiles.Remove(this);
            }
        }
    }

    /// <summary>
    /// Spreads its own <see cref="Taint"/> to its neighbouring <see cref="Tile"/>s.
    /// Returns a list of <see cref="Tile"/>s that were (possibly) affected by the taint-spreading from this <see cref="Tile"/>.
    /// 
    /// This is done in the following order: Top, Bottom, Left, Right.
    /// 
    /// Developer comment: I am pretty sure the return result should NOT contain duplicates but I am not 100% sure of this.
    /// </summary>
    /// <returns>A list of <see cref="Tile"/>s after spreading the taint. NOTE: this list may contain 'null' elements.</returns>
    public List<Tile> TaintNeighbouringTiles() {
        List<Tile> newTaintedTiles = new List<Tile>();

        if (isTainted) {
            if (tileCoords.x - 1 >= 0) {
                Tile tileToTaint = gameLevel.GetMap()[tileCoords.x - 1][tileCoords.y];
                if (tileToTaint != null) newTaintedTiles.Add(tileToTaint.Taint(true));
            }

            if (tileCoords.x + 1 < gameLevel.GetMap().Length) {
                Tile tileToTaint = gameLevel.GetMap()[tileCoords.x + 1][tileCoords.y];
                if (tileToTaint != null) newTaintedTiles.Add(tileToTaint.Taint(true));
            }

            if (tileCoords.y - 1 >= 0) {
                Tile tileToTaint = gameLevel.GetMap()[tileCoords.x][tileCoords.y - 1];
                if (tileToTaint != null) newTaintedTiles.Add(tileToTaint.Taint(true));
            }

            if (tileCoords.y + 1 < gameLevel.GetMap()[tileCoords.x].Length) {
                Tile tileToTaint = gameLevel.GetMap()[tileCoords.x][tileCoords.y + 1];
                if (tileToTaint != null) newTaintedTiles.Add(tileToTaint.Taint(true));
            }
        }

        newTaintedTiles.RemoveAll(t => t == null);
        return newTaintedTiles;
    }

    /// <summary>
    /// This makes a specified tile aware of the object that is placed on it.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void PlaceOnTile(Mergeable obj, Tile t) {
        t.objectOnTile = obj;
    }

    /// <summary>
    /// This performs the Mergeable object reference-handling when that object is moved from one tile to another,
    /// and physically moves that object if possible.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void MoveToTile(Mergeable obj, Tile from, Tile to) {
        RemoveFromTile(obj, from);
        PlaceOnTile(obj, to);
        obj.transform.position = to.transform.position;
        obj.CurrentLocation = to;
    }

    /// <summary>
    /// This makes a specified tile aware that a Mergeable object has been removed from it.
    /// 
    /// Note that this does not actually physically affect the Mergeable object.
    /// If destruction of that object is necessary, use <see cref="RemoveFromTileAndDestroy(Mergeable, Tile)"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void RemoveFromTile(Mergeable obj, Tile t) {
        if (t.objectOnTile == obj) {
            t.objectOnTile = null;
        }
    }

    /// <summary>
    /// This makes a specified tile aware that a Mergeable object has been removed from it.
    /// 
    /// Note that this also physically destroys the Mergeable object.
    /// If destruction of that object is not necessary, use <see cref="RemoveFromTile(Mergeable, Tile)"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void RemoveFromTileAndDestroy(Mergeable obj, Tile t) {
        if (t.objectOnTile == obj) {
            FindObjectOfType<ObjectSpawner>().RemoveObjectFromGame(obj.gameObject);
            t.objectOnTile = null;
        }
    }

    void PlayParticleEffect(CustomEnums.TileType tileType) {        
        switch (tileType) {
            case CustomEnums.TileType.PURIFIED:
            case CustomEnums.TileType.INVULNERABLE:
                tilePurifyParticleSystem.Play();
                break;
            case CustomEnums.TileType.TAINTED:
                tileTaintParticleSystem.Play();
                break;
            default:
                throw new System.Exception("Invalid TileType enum encountered when setting up particle effect to play.");
        }
    }
}
