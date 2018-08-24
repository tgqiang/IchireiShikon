using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class acts as a data structure encapsulating all required data of a game level.
/// </summary>
public class LevelData {

    /// <summary>
    /// The representation of the tiles in a game level.
    /// Tiles in this map are accessed with {row, column} coordinates, where 'x' denotes row and 'y' denotes column.
    /// </summary>
    public Tile[][] tileMap;
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
