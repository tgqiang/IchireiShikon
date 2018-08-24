using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mergeable : MonoBehaviour {

    public const int SPIRIT_LEVEL_MAX = 4;

    /// <summary>
    /// Rules for tainted objects:
    /// 1. Tainted souls are destroyed (or made immobile) - implementation not certain for now,
    /// 2. Tainted spirits remain in-game, but will taint tiles in accordance to their AoE when triggered.
    /// </summary>
    protected bool isTainted;

    /// <summary>
    /// This attribute is used to determine which Mergeable object was acted on by player input.
    /// </summary>
    protected bool isSelected;

    protected Tile currentLocation;

    public Sprite[] sprites;

    public virtual void InitializeLocation(Tile t) {
        currentLocation = t;
    }

    public virtual void Taint() {
        isTainted = true;
    }

    public virtual void Purify() {
        isTainted = false;
    }

    public virtual List<Mergeable> GetConnectedObjects(List<Mergeable> result, Mergeable triggeringObject) {
        if (!result.Contains(this)) result.Add(this);

        Tile[][] tilemap = Tile.gameLevel.GetMap();

        if (currentLocation.tileCoords.x - 1 >= 0) {
            Tile top = tilemap[currentLocation.tileCoords.x - 1][currentLocation.tileCoords.y];
            if (top != null) {
                if (triggeringObject.IsSameTypeAs(top.objectOnTile)) {
                    result = GetConnectedObjects(result, top.objectOnTile);
                }
            }
        }

        if (currentLocation.tileCoords.x + 1 < tilemap.Length) {
            Tile bottom = tilemap[currentLocation.tileCoords.x + 1][currentLocation.tileCoords.y];
            if (bottom != null) {
                if (triggeringObject.IsSameTypeAs(bottom.objectOnTile)) {
                    result = GetConnectedObjects(result, bottom.objectOnTile);
                }
            }
        }

        if (currentLocation.tileCoords.y - 1 >= 0) {
            Tile left = tilemap[currentLocation.tileCoords.x - 1][currentLocation.tileCoords.y];
            if (left != null) {
                if (triggeringObject.IsSameTypeAs(left.objectOnTile)) {
                    result = GetConnectedObjects(result, left.objectOnTile);
                }
            }
        }

        if (currentLocation.tileCoords.y + 1 < tilemap[currentLocation.tileCoords.x].Length) {
            Tile right = tilemap[currentLocation.tileCoords.x + 1][currentLocation.tileCoords.y];
            if (right != null) {
                if (triggeringObject.IsSameTypeAs(right.objectOnTile)) {
                    result = GetConnectedObjects(result, right.objectOnTile);
                }
            }
        }

        return result;
    }

    public virtual void Merge() {
        List<Mergeable> connectedObjects = GetConnectedObjects(new List<Mergeable>(), this);
        int numMergedObjects = connectedObjects.Count;
        foreach (Mergeable obj in connectedObjects) {
            Tile.RemoveFromTileAndDestroy(obj, obj.currentLocation);
        }
        SpawnObjectOnMerge(this, numMergedObjects);
    }

    public virtual void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        // Empty body, to be overriden by subclasses.
    }

    public virtual bool IsSameTypeAs(Mergeable other) {
        return this == other;
    }
}
