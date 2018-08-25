using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mergeable : MonoBehaviour {

    public const int SPIRIT_LEVEL_MAX = 4;
    public const int MIN_OBJECTS_FOR_MERGE = 3;

    /// <summary>
    /// Rules for tainted objects:
    /// 1. Tainted souls are destroyed (or made immobile) - implementation not certain for now,
    /// 2. Tainted spirits remain in-game, but will taint tiles in accordance to their AoE when triggered.
    /// </summary>
    protected bool isTainted;
    public bool IsTainted {
        get { return isTainted; }
    }

    /// <summary>
    /// This attribute checks for which tile this object is on.
    /// </summary>
    protected Tile currentLocation;
    public Tile CurrentLocation {
        get { return currentLocation; }
        set { currentLocation = value; }
    }

    public Sprite[] sprites;

    public virtual void Taint() {
        isTainted = true;
    }

    public virtual void Purify() {
        isTainted = false;
    }

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

    public virtual void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        // Empty body, to be overriden by subclasses.
    }

    /// <summary>
    /// Determines the level of the spawned spirit after souls/spirits have been merged together.
    /// 
    /// If a spirit is spawned from merging of souls, 'mergedObjectLevel' argument should not be supplied.
    /// </summary>
    /// <param name="mergedObjectCount">The number of objects used in the merge.</param>
    /// <param name="mergedObjectLevel">The level of the spirit objects used in the merge, if they are merged to form a higher-level spirit.</param>
    /// <returns>The computed level of the spirit to be spawned after the merge.</returns>
    public virtual int DetermineSpawnedSpiritLevel(int mergedObjectCount, int mergedObjectLevel = 0) {
        return mergedObjectLevel + Mathf.Min(Mathf.FloorToInt((mergedObjectCount - 1) / 2), SPIRIT_LEVEL_MAX);
    }

    public virtual bool IsSameTypeAs(Mergeable other) {
        return false;
    }
}
