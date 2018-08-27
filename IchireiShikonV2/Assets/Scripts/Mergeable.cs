using System.Collections.Generic;
using UnityEngine;

public class Mergeable : MonoBehaviour {

    public const int SPIRIT_LEVEL_MAX = 4;
    public const int MIN_OBJECTS_FOR_MERGE = 3;

    /// <summary>
    /// Rules for tainted objects:
    /// 1. Tainted <seealso cref="Soul"/>s are destroyed (or made immobile) - implementation not certain for now,
    /// 2. Tainted <seealso cref="Spirit"/>s remain in-game, but will taint tiles in accordance to their AoE when triggered.
    /// </summary>
    protected bool isTainted;
    public bool IsTainted {
        get { return isTainted; }
    }

    /// <summary>
    /// This attribute checks for which <seealso cref="Tile"/> this object is on.
    /// </summary>
    protected Tile currentLocation;
    public Tile CurrentLocation {
        get { return currentLocation; }
        set { currentLocation = value; }
    }

    public Sprite[] sprites;

    /// <summary>
    /// <see cref="Mergeable"/> objects can be tainted when the taint from the <seealso cref="Tile"/>s spread to other <seealso cref="Tile"/>s.
    /// 
    /// This function is made 'virtual' since different types of <see cref="Mergeable"/> objects behave differently upon being tainted.
    /// See <see cref="isTainted"/> for the specifications of these behaviours.
    /// </summary>
    public virtual void Taint() {
        isTainted = true;
    }

    public virtual void Purify() {
        isTainted = false;
    }

    /// <summary>
    /// Queries for all <see cref="Mergeable"/> objects that are connected to this one, when a merge operation is requested.
    /// A <see cref="Mergeable"/> object is deemed as connected to this one if it is of the same type as this one.
    /// 
    /// This query uses a depth-first search algorithm in the following order: Top, Bottom, Left, Right.
    /// </summary>
    /// <param name="result">A list for recording all connected <see cref="Mergeable"/> objects detected.</param>
    /// <param name="tileMap">The tile map of the current level, obtainable from <seealso cref="Level.levelData"/>.</param>
    /// <param name="tilemapBounds">The bounds of the current level's tile map, obtainable from <seealso cref="Level.levelData"/>.</param>
    /// <returns>A list of <see cref="Mergeable"/> objects that are connected to this one.</returns>
    public virtual List<Mergeable> GetConnectedObjects(List<Mergeable> result, Tile[][] tileMap, Vector2Int tilemapBounds) {
        if (!result.Contains(this)) {
            result.Add(this);

            if (currentLocation.tileCoords.x - 1 >= 0) {
                Tile top = tileMap[CurrentLocation.tileCoords.x - 1][CurrentLocation.tileCoords.y];
                if (top != null) {
                    if (top.objectOnTile != null && IsSameTypeAs(top.objectOnTile)) {
                        result = top.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                    }
                }
            }

            if (currentLocation.tileCoords.x + 1 < tilemapBounds.x) {
                Tile bottom = tileMap[CurrentLocation.tileCoords.x + 1][CurrentLocation.tileCoords.y];
                if (bottom != null) {
                    if (bottom.objectOnTile != null && IsSameTypeAs(bottom.objectOnTile)) {
                        result = bottom.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                    }
                }
            }

            if (currentLocation.tileCoords.y - 1 >= 0) {
                Tile left = tileMap[CurrentLocation.tileCoords.x][CurrentLocation.tileCoords.y - 1];
                if (left != null) {
                    if (left.objectOnTile != null && IsSameTypeAs(left.objectOnTile)) {
                        result = left.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                    }
                }
            }

            if (currentLocation.tileCoords.y + 1 < tilemapBounds.y) {
                Tile right = tileMap[CurrentLocation.tileCoords.x][CurrentLocation.tileCoords.y + 1];
                if (right != null) {
                    if (right.objectOnTile != null && IsSameTypeAs(right.objectOnTile)) {
                        result = right.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// When 3 or more <see cref="Mergeable"/> objects are connected, they can merge to produce a <see cref="Spirit"/> object.
    /// </summary>
    public virtual void Merge() {
        List<Mergeable> connectedObjects = GetConnectedObjects(new List<Mergeable>(),
            Tile.gameLevel.levelData.tileMap, Tile.gameLevel.levelData.mapBounds);
        int numMergedObjects = connectedObjects.Count;
        if (numMergedObjects >= MIN_OBJECTS_FOR_MERGE) {
            foreach (Mergeable obj in connectedObjects) {
                Tile.RemoveFromTileAndDestroy(obj, obj.CurrentLocation);
            }
            SpawnObjectOnMerge(this, numMergedObjects);
        }
    }

    /// <summary>
    /// This function in <see cref="Mergeable"/> is empty because <seealso cref="Soul"/>s and <seealso cref="Spirit"/>s
    /// employ somewhat-different formula on merge (see <see cref="DetermineSpawnedSpiritLevel(int, int)"/>).
    /// 
    /// This function is intended to be overriden in the <seealso cref="Soul"/> and <seealso cref="Spirit"/> subclasses.
    /// </summary>
    /// <param name="triggeringObject">The <see cref="Mergeable"/> object that triggered the merge.</param>
    /// <param name="mergedObjectCount">The number of <see cref="Mergeable"/> objects merged, obtainable from <see cref="GetConnectedObjects(List{Mergeable}, Tile[][], Vector2Int)"/>.</param>
    public virtual void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        // Empty body, to be overriden by subclasses.
    }

    /// <summary>
    /// Determines the level of the spawned <seealso cref="Spirit"/> after <seealso cref="Soul"/>s/<seealso cref="Spirit"/>s have been merged together.
    /// 
    /// If a <seealso cref="Spirit"/> is spawned from merging of <seealso cref="Soul"/>s, 'mergedObjectLevel' argument should not be supplied.
    /// </summary>
    /// <param name="mergedObjectCount">The number of objects used in the merge.</param>
    /// <param name="mergedObjectLevel">The level of the spirit objects used in the merge, if they are merged to form a higher-level spirit.</param>
    /// <returns>The computed level of the spirit to be spawned after the merge.</returns>
    public virtual int DetermineSpawnedSpiritLevel(int mergedObjectCount, int mergedObjectLevel = 0) {
        return mergedObjectLevel + Mathf.Min(Mathf.FloorToInt((mergedObjectCount - 1) / 2), SPIRIT_LEVEL_MAX);
    }

    /// <summary>
    /// This is the filtering function used in the object-connectivity query, <see cref="GetConnectedObjects(List{Mergeable}, Tile[][], Vector2Int)"/>.
    /// 
    /// To qualify as being of the same type, <seealso cref="Soul"/>s simply need to share <seealso cref="Soul.soulType"/>, whereas
    /// <seealso cref="Spirit"/>s must share the same <seealso cref="Spirit.spiritType"/> and <seealso cref="Spirit.spiritLevel"/>.
    /// </summary>
    /// <param name="other">The <see cref="Mergeable"/> object, for comparing of type.</param>
    /// <returns>True if 'other' is of the same type as this one; False otherwise.</returns>
    public virtual bool IsSameTypeAs(Mergeable other) {
        return false;
    }
}
