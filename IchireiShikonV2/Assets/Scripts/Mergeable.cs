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

    protected Tile currentLocation;

    public Sprite[] sprites;
    
    public virtual Tile GetLocation() {
        return currentLocation;
    }

    public virtual void SetLocation(Tile t) {
        currentLocation = t;
    }

    public virtual void Taint() {
        isTainted = true;
    }

    public virtual void Purify() {
        isTainted = false;
    }

    public virtual List<Mergeable> GetConnectedObjects(List<Mergeable> result, Tile[][] tileMap, Vector2Int tilemapBounds) {
        if (!result.Contains(this)) {
            result.Add(this);
        }

        if (currentLocation.tileCoords.x - 1 >= 0) {
            Tile top = tileMap[currentLocation.tileCoords.x - 1][currentLocation.tileCoords.y];
            if (top != null) {
                if (top.objectOnTile != null && IsSameTypeAs(top.objectOnTile) && !result.Contains(top.objectOnTile)) {
                    result = top.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                }
            }
        }

        if (currentLocation.tileCoords.x + 1 < tilemapBounds.x) {
            Tile bottom = tileMap[currentLocation.tileCoords.x + 1][currentLocation.tileCoords.y];
            if (bottom != null) {
                if (bottom.objectOnTile != null && IsSameTypeAs(bottom.objectOnTile) && !result.Contains(bottom.objectOnTile)) {
                    result = bottom.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                }
            }
        }

        if (currentLocation.tileCoords.y - 1 >= 0) {
            Tile left = tileMap[currentLocation.tileCoords.x][currentLocation.tileCoords.y - 1];
            if (left != null) {
                if (left.objectOnTile != null && IsSameTypeAs(left.objectOnTile) && !result.Contains(left.objectOnTile)) {
                    result = left.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
                }
            }
        }

        if (currentLocation.tileCoords.y + 1 < tilemapBounds.y) {
            Tile right = tileMap[currentLocation.tileCoords.x][currentLocation.tileCoords.y + 1];
            if (right != null) {
                if (right.objectOnTile != null && IsSameTypeAs(right.objectOnTile) && !result.Contains(right.objectOnTile)) {
                    result = right.objectOnTile.GetConnectedObjects(result, tileMap, tilemapBounds);
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
                Tile.RemoveFromTileAndDestroy(obj, obj.GetLocation());
            }
            SpawnObjectOnMerge(this, numMergedObjects);
        }
    }

    public virtual void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        // Empty body, to be overriden by subclasses.
    }

    public virtual bool IsSameTypeAs(Mergeable other) {
        return false;
    }
}
