using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as the base template for tile-specific <seealso cref="Spirit"/>-effects, where necessary.
/// </summary>
public class SpiritEffect : MonoBehaviour {

    /// <summary>
    /// A coordinate-offset vector, in the form of {row, column}.
    /// 
    /// This attribute is used to obtain the tile that this effect will take effect on,
    /// with respect to the triggering spirit object.
    /// 
    /// See also <seealso cref="Tile.tileCoords"/>.
    /// </summary>
    [SerializeField]
    protected Vector2Int offsetFromCenter;

    public virtual void ExertEffect(Vector2Int centerCoords, Vector2Int mapBounds, Tile[][] tileMap) {
        Vector2Int targetCoords = centerCoords + offsetFromCenter;

        if (targetCoords.x.IsBetweenExclusive(0, mapBounds.x) &&
            targetCoords.y.IsBetweenExclusive(0, mapBounds.y)) {
            Tile targetTile = tileMap[targetCoords.x][targetCoords.y];
            if (targetTile != null) {
                if (GetComponentInParent<Spirit>().IsTainted) {
                    targetTile.Taint();
                } else {
                    PerformEffect(targetTile);
                }
            }
        }
    }

    public virtual void PerformEffect(Tile target) {
        // Empty body, to be overriden by subclasses.
    }
}
