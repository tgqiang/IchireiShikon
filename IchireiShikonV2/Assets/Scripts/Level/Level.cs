using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is strictly required for any level scene, and serves as a kind of "blackboard"
/// that contains the necessary data that is required by the various game objects in the level.
/// 
/// It's only active role is to check for game victory or game-over conditions.
/// 
/// The [RequireComponent] decorator will ensure that the other important scripts
/// needed for the current level are added accordingly.
/// </summary>
[RequireComponent(typeof(LevelConstructor), typeof(ObjectSpawner))]
public class Level : MonoBehaviour {

    /// <summary>
    /// The level of the game's scene, which is 1-based.
    /// 
    /// Note that all level's CSV files are to be named L{<see cref="level"/>}.csv.
    /// </summary>
    [SerializeField]
    int level;

    /// <summary>
    /// This is the data structure containing all required information.
    /// 
    /// For further details about this data structure, see <seealso cref="LevelData"/>.
    /// </summary>
    public LevelData levelData;

	void Awake () {
        LevelConstructor constructor = FindObjectOfType<LevelConstructor>();
        if (constructor == null) {
            throw new System.Exception("Level constructor script is missing from game scene.");
        }

        levelData = LevelConstructor.BuildLevel(LevelConstructor.ParseLevel(level));
        if (levelData == null) {
            throw new System.Exception("Level data is missing. Please check LevelConstructor script.");
        }
	}

    void Update() {
        if (levelData.taintedTiles.Count == levelData.totalTiles) {
            // TODO: implement game-over state.
        } else if (levelData.taintedTiles.IsNullOrEmpty()) {
            // TODO: implement victory state.
        }
    }

    /// <summary>
    /// Accesses the tile map of the current level.
    /// </summary>
    /// <returns>A <seealso cref="Tile"/>-2D-array representing the current level's tile map.</returns>
    public Tile[][] GetMap() {
        return levelData.tileMap;
    }
}
