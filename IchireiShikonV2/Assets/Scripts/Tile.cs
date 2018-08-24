using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public static Level gameLevel;

    /// <summary>
    /// The coordinates on the tile map in {row, column} notation, where 'x' denotes row and 'y' denotes column.
    /// 
    /// Row and column counting starts from top-left corner of the tile map.
    /// </summary>
    public Vector2Int tileCoords;

    public bool isTainted;
    public bool isPurified;
    public bool isInvulnerable;

    public Mergeable objectOnTile;

    public Sprite[] tileSprites;

    void OnEnable() {
        gameLevel = FindObjectOfType<Level>();
    }

    public bool IsVacant(Mergeable obj) {
        return objectOnTile == null || objectOnTile == obj;
    }

    public void Taint() {
        if (!isInvulnerable) {
            if (!isTainted) {
                isTainted = true;
                isPurified = false;
                GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.TAINTED];

                if (objectOnTile != null) {
                    if (objectOnTile is Soul) {
                        RemoveFromTileAndDestroy(objectOnTile, this);
                    } else if (objectOnTile is Spirit) {
                        objectOnTile.Taint();
                    }
                }

                if (!gameLevel.levelData.taintedTiles.Contains(this)) {
                    gameLevel.levelData.taintedTiles.Add(this);
                }
            }
        }
    }

    public void Purify() {
        if (isTainted) {
            isTainted = false;
            isPurified = true;
            isInvulnerable = false;
            GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.PURIFIED];

            if (gameLevel.levelData.taintedTiles.Contains(this)) {
                gameLevel.levelData.taintedTiles.Remove(this);
            }
        }
    }

    public void PurifyWithBarrier() {
        if (isTainted) {
            isTainted = false;
            isPurified = true;
            isInvulnerable = true;
            GetComponent<SpriteRenderer>().sprite = tileSprites[(int) CustomEnums.TileType.INVULNERABLE];

            if (gameLevel.levelData.taintedTiles.Contains(this)) {
                gameLevel.levelData.taintedTiles.Remove(this);
            }
        }
    }

    public void TaintNeighbouringTiles() {
        if (isTainted) {
            if (tileCoords.x - 1 >= 0) { gameLevel.GetMap()[tileCoords.x - 1][tileCoords.y].Taint(); }
            if (tileCoords.x + 1 < gameLevel.GetMap().Length) { gameLevel.GetMap()[tileCoords.x + 1][tileCoords.y].Taint(); }
            if (tileCoords.y - 1 >= 0) { gameLevel.GetMap()[tileCoords.x][tileCoords.y - 1].Taint(); }
            if (tileCoords.y + 1 < gameLevel.GetMap()[tileCoords.x].Length) { gameLevel.GetMap()[tileCoords.x][tileCoords.y + 1].Taint(); }
        }
    }

    /// <summary>
    /// This makes a specified tile aware of the object that is placed on it.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void PlaceOnTile(Mergeable obj, Tile t) {
        t.objectOnTile = obj;
    }

    /// <summary>
    /// This performs the Mergeable object reference-handling when that object is moved from one tile to another.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void MoveToTile(Mergeable obj, Tile from, Tile to) {
        PlaceOnTile(obj, to);
        RemoveFromTile(obj, from);
    }

    /// <summary>
    /// This makes a specified tile aware that a Mergeable object has been removed from it.
    /// 
    /// Note that this does not actually physically affect the Mergeable object.
    /// If destruction of that object is necessary, use <see cref="RemoveFromTileAndDestroy(Mergeable, Tile)"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void RemoveFromTile(Mergeable obj, Tile t) {
        if (t.objectOnTile == obj) {
            t.objectOnTile = null;
        }
    }

    /// <summary>
    /// This makes a specified tile aware that a Mergeable object has been removed from it.
    /// 
    /// Note that this also physically destroys the Mergeable object.
    /// If destruction of that object is not necessary, use <see cref="RemoveFromTile(Mergeable, Tile)"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    public static void RemoveFromTileAndDestroy(Mergeable obj, Tile t) {
        if (t.objectOnTile == obj) {
            FindObjectOfType<ObjectSpawner>().RemoveObjectFromGame(obj.gameObject);
            t.objectOnTile = null;
        }
    }
}
