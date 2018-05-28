using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    [SerializeField]
    int totalNumberOfTiles;
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
        InitializeTileSprites();
        InitializeTileManagerForRequiredClasses();
        UpdateTileRecords();
    }

    void Update () {
        if (taintedTiles.Count == 0) {
            TriggerVictory();
        } else if (taintedTiles.Count == totalNumberOfTiles) {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Initializes the 3 types of tile sprites needed to reflect the 3 different tile states.
    /// </summary>
    private void InitializeTileSprites () {
        AssertCorrectTileSprites();
        Tile.tileSprites = tileSprites;
    }

    private void AssertCorrectTileSprites () {
        Debug.Assert(tileSprites != null, "'tileSprites' in TileManager is not initialized.");
        Debug.Assert(tileSprites.Length == Configurable.NUM_TILE_SPRITES, "There are incorrect number of tile sprites in TileManager.");
        Debug.Assert(tileSprites[(int) Tile.SpriteIndices.UNTAINTED].name == Configurable.instance.UNTAINTED_TILE_NAME, "Untainted tile sprite in TileManager is incorrect.");
        Debug.Assert(tileSprites[(int) Tile.SpriteIndices.SHIELDED].name == Configurable.instance.SHIELDED_TILE_NAME, "Invulnerable tile sprite in TileManager is incorrect.");
        Debug.Assert(tileSprites[(int) Tile.SpriteIndices.TAINTED].name == Configurable.instance.TAINTED_TILE_NAME, "Tainted tile sprite in TileManager is incorrect.");
    }

    /// <summary>
    /// Sets the required static TileManager instance in classes that explicitly requires this instance.
    /// </summary>
    private void InitializeTileManagerForRequiredClasses () {
        Soul.tileManager = this;
        Spirit.tileManager = this;
        Tile.tileManager = this;
    }

    /// <summary>
    /// Primarily, this function updates the list of tainted tiles.
    /// This will also retrieve the total number of tiles present in the level.
    /// </summary>
    private void UpdateTileRecords () {
        Tile[] tilesInLevel = GameObject.FindObjectsOfType<Tile>();
        totalNumberOfTiles = tilesInLevel.Length;
        taintedTiles = new List<Tile>(tilesInLevel).FindAll(t => t.IsTainted);
    }

    public void RemoveTileFromTaintedList (Tile t) {
        if (taintedTiles.Contains(t)) { taintedTiles.Remove(t); }
    }

    public void UpdateTiles () {
        if (hasTakenMove) {
            foreach (Tile t in taintedTiles) {
                t.Spread();
            }

            UpdateTileRecords ();
            hasTakenMove = false;
        }
    }

    public void TakeMove () {
        hasTakenMove = true;
        UpdateCameraBackgroundColor();
        UpdateTiles();
    }

    void UpdateCameraBackgroundColor () {
        Configurable.instance.mainCamera.backgroundColor = Color.Lerp(Configurable.instance.colorVictory, Configurable.instance.colorGameOver, (float) taintedTiles.Count / totalNumberOfTiles);
    }

    private void TriggerVictory () {
        // TODO: implement victory screen
    }

    private void TriggerGameOver () {
        // TODO: implement game over screen
    }
}
