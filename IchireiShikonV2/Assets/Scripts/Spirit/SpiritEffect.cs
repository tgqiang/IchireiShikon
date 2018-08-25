using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritEffect : MonoBehaviour {

    [SerializeField]
    Vector2Int offsetFromCenter;

    public virtual void ExertEffect(Vector2Int centerCoords, Vector2Int mapBounds, Tile[][] tileMap) {
        Vector2Int targetCoords = centerCoords + offsetFromCenter;

        if (targetCoords.x.IsBetweenExclusive(0, mapBounds.x) &&
            targetCoords.y.IsBetweenExclusive(0, mapBounds.y)) {
            Tile targetTile = tileMap[targetCoords.x][targetCoords.y];
            if (targetTile != null) PerformEffect(targetTile);
        }
    }

    public virtual void PerformEffect(Tile target) {
        // Empty body, to be overriden by subclasses.
    }
}
