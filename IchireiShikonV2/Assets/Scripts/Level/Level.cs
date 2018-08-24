using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelConstructor))]
public class Level : MonoBehaviour {

    [SerializeField]
    int level;

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

    public Tile[][] GetMap() {
        return levelData.tileMap;
    }
}
