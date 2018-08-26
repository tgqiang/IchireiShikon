using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a data structure encapsulating all required data of a game level.
/// </summary>
/// 
/// 4 pieces of information are critical for the core systems of this game, which are:
/// 1. Tile map of the level
/// 2. Maximal boundaries of the tile map
/// 3. List of tainted tiles in the level
/// 4. Total number of tiles in the level
public class LevelData {

    /// <summary>
    /// The representation of the tiles in a game level.
    /// Tiles in this map are accessed with {row, column} coordinates, where 'x' denotes row and 'y' denotes column.
    /// 
    /// See also <seealso cref="Tile.tileCoords"/>.
    /// </summary>
    public Tile[][] tileMap;
    /// <summary>
    /// Contains the maximal boundaries of the tile map, in the form of {MAX_ROWS, MAX_COLUMNS}.
    /// 
    /// See also <seealso cref="Tile.tileCoords"/>.
    /// </summary>
    public Vector2Int mapBounds;
    /// <summary>
    /// A list that tracks all the tainted tiles that exist in the level.
    /// This is required for performing the taint-spreading feature.
    /// </summary>
    public List<Tile> taintedTiles = new List<Tile>();
    /// <summary>
    /// This attribute is required for checking if the player has lost the current level.
    /// Game-over condition triggers when all tiles in the level become tainted.
    /// </summary>
    public int totalTiles;
}
