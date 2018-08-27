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
    /// A flag for controlling the execution of the game-over/victory state.
    /// 
    /// We need this flag since the game-over/victory state is constantly checked in the <see cref="Update"/> loop
    /// and we only want to execute these states ONCE.
    /// </summary>
    bool isGameTerminationTriggered;

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
        if (levelData.taintedTiles.Count >= levelData.totalTiles) {
            // TODO: implement game-over state.
            if (!isGameTerminationTriggered) {
                Debug.Log("Game is over.");
                isGameTerminationTriggered = true;
            }
        } else if (levelData.taintedTiles.Count <= 0) {
            // TODO: implement victory state.
            if (!isGameTerminationTriggered) {
                Debug.Log("Victory!");
                isGameTerminationTriggered = true;
            }
        }
    }

    /// <summary>
    /// Accesses the tile map of the current level.
    /// </summary>
    /// <returns>A <seealso cref="Tile"/>-2D-array representing the current level's tile map.</returns>
    public Tile[][] GetMap() {
        return levelData.tileMap;
    }

    /// <summary>
    /// Updates the list of tainted tiles in the current level.
    /// This function will also ensure that duplicates are not re-added back into the record of tainted tiles.
    /// 
    /// 'null' and duplicate elements are guaranteed to be absent since those are stripped off by
    /// <seealso cref="Tile.Taint"/> and <seealso cref="Tile.TaintNeighbouringTiles"/>.
    /// 
    /// See <seealso cref="LevelData.taintedTiles"/>.
    /// </summary>
    /// <param name="newTaintedTiles"></param>
    public void UpdateTaintedTilesRecord(List<Tile> newTaintedTiles) {
        levelData.taintedTiles.AddRange(newTaintedTiles);
    }
}
