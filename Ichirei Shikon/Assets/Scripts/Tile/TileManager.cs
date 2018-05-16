using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    
    /// <summary>
    /// Contains the required sprites for the various tile types.
    /// Convention for tileSprites:
    /// [0] - Untainted tile,
    /// [1] - Untainted tile with barrier,
    /// [2] - Tainted tile
    /// </summary>
    [SerializeField]
    Sprite[] tileSprites;

    /// <summary>
    /// Stores all the tainted tiles that are in the current level.
    /// </summary>
    [SerializeField]
    List<Tile> taintedTiles;

    /// <summary> A flag for controlling the moment when every tile is updated. </summary> ///
    bool hasTakenMove = false;
    public bool HasTakenMove {
        get { return hasTakenMove; }
        set { hasTakenMove = value; }
    }

	// Use this for initialization
	void Start () {
        InitializeTileProperties();
        InitializeSoulProperties();
        UpdateTaintedTilesList();
    }

    /// <summary>
    /// Sets the required static attributes/references in the Tile class.
    /// </summary>
    private void InitializeTileProperties () {
        Tile.tileManager = this;
        Tile.tileSprites = tileSprites;
    }

    private void InitializeSoulProperties () {
        Soul.tileManager = this;
    }

    private void UpdateTaintedTilesList () {
        taintedTiles = new List<Tile>(GameObject.FindObjectsOfType<Tile>()).FindAll(t => t.IsTainted);
    }

    public void RemoveTileFromTaintedList (Tile t) {
        if (taintedTiles.Contains(t)) { taintedTiles.Remove(t); }
    }

    public void UpdateTiles () {
        if (hasTakenMove) {
            foreach (Tile t in taintedTiles) {
                t.Spread();
            }

            UpdateTaintedTilesList ();
            hasTakenMove = false;
        }
    }

    public void TakeMove () {
        hasTakenMove = true;
        UpdateTiles();
    }
}
