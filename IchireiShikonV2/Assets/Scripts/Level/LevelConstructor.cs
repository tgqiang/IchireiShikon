﻿using System;
using UnityEngine;

/// <summary>
/// This component is responsible for processing the level data in a level's associating CSV file,
/// and constructing the level physically in the game.
/// 
/// This component also requires a <seealso cref="ObjectSpawner"/> for spawning of game objects when
/// constructing the level physically in-game.
/// </summary>
public class LevelConstructor : MonoBehaviour {
    
    const string LEVEL_FILE_PATH = "/LevelData/L{0}.csv";
    const char DELIMITER = ',';

    const int LEVEL_TILEMAP_MAX_LENGTH = 17;
    const int LEVEL_TILEMAP_MAX_HEIGHT = 9;

    /// <summary>
    /// The game-object spawner, that is required for spawning physical game-objects in the game.
    /// 
    /// For more information about this component, see <seealso cref="ObjectSpawner"/>.
    /// </summary>
    static ObjectSpawner spawner;

    void Awake() {
        spawner = FindObjectOfType<ObjectSpawner>();
        if (spawner == null) {
            throw new Exception("The required ObjectSpawner component for constructing the level is missing.");
        }
    }

    /// <summary>
    /// Parses the contents in a desired level's CSV file into a 2D-int-array, which is subsequently used for level-construction later on.
    /// </summary>
    /// <param name="level">The desired level to build, 1-indexed.</param>
    /// <returns>The level data, represented in a 2D-int-array.</returns>
    public static int[][] ParseLevel(int level) {
        string[] rawData = ReadLevelDataFromFile(level);

        if (rawData.Length > LEVEL_TILEMAP_MAX_HEIGHT) {
            throw new System.Exception("Level map design exceeds height limit of " + LEVEL_TILEMAP_MAX_HEIGHT + " elements.");
        }

        int[][] decoded = new int[rawData.Length][];
        for (int i = 0; i < rawData.Length; i++) {
            string[] l = rawData[i].Split(',');
            int[] row = new int[l.Length];

            if (l.Length > LEVEL_TILEMAP_MAX_LENGTH) {
                throw new System.Exception("Level map design exceeds length limit of " + LEVEL_TILEMAP_MAX_LENGTH + " elements.");
            }

            for (int j = 0; j < l.Length; j++) {
                row[j] = int.Parse(l[j]);
                if (row[j] >= LevelSyntax.MAX_NUMERICAL_REPRESENTATION) {
                    throw new System.Exception("Number of bits present in the cell exceeds maximum format constraints.");
                }
            }

            decoded[i] = row;
        }

        return decoded;
    }

    private static string[] ReadLevelDataFromFile(int level) {
        string levelToDecodePath = Application.streamingAssetsPath + LEVEL_FILE_PATH.Formatted(level);
        return levelToDecodePath.LoadFrom_AsString().Split('\n');
    }

    /// <summary>
    /// Constructs the level, given the data of the tile map obtained after parsing the CSV file.
    /// </summary>
    /// <param name="tilemapData">The parsed tile map data.</param>
    /// <returns>All data required for the level, represented as a LevelData object.</returns>
    public static LevelData BuildLevel(int[][] tilemapData) {
        LevelData data = new LevelData {
            tileMap = new Tile[tilemapData.Length][],
            mapBounds = new Vector2Int(tilemapData.Length, tilemapData[0].Length)
        };


        for (int row = 0; row < tilemapData.Length; row++) {
            data.tileMap[row] = new Tile[tilemapData[row].Length];

            for (int col = 0; col < tilemapData[row].Length; col++) {
                if (tilemapData[row][col] == LevelSyntax.EMPTY_UNIT) continue;

                // Update counter of total number of tiles
                data.totalTiles += 1;

                // Tile-spawning at respective row-column coordinates
                int tileType = tilemapData[row][col] & LevelSyntax.MASK_TILE;
                GameObject tileObj = spawner.SpawnTile(tileType, spawner.transform.position + new Vector3(col, -row));
                Tile currentTile = tileObj.GetComponent<Tile>();

                // Setting spawned tile's coordinates
                currentTile.tileCoords = new Vector2Int(row, col);

                // Setting entries in tileMap
                data.tileMap[row][col] = currentTile;

                // Updating record of tainted tiles
                if (tileType == (int) CustomEnums.TileType.TAINTED) data.taintedTiles.Add(currentTile);

                // Soul/Spirit spawning and initializations
                int soulType = (tilemapData[row][col] & LevelSyntax.MASK_SOUL) >> LevelSyntax.BITSHIFT_SOUL;
                int spiritType = (tilemapData[row][col] & LevelSyntax.MASK_SPIRIT) >> LevelSyntax.BITSHIFT_SPIRIT;

                // Exception in data #1
                if (Mathf.Min(soulType, spiritType) > LevelSyntax.NONE) {
                    throw new System.Exception("A tile unit cannot contain more than 1 Mergeable object; " +
                        "Encountered soulType = [" + soulType + "], spiritType = [" + spiritType + "].");
                }

                // Exception in data #2-A
                if (soulType > Enum.GetNames(typeof(CustomEnums.SoulType)).Length) {
                    throw new System.Exception("Invalid soulType requested for during level construction; " +
                        "Encountered soulType = [" + soulType + "].");
                }

                if (soulType > LevelSyntax.NONE) {
                    GameObject soulObj = spawner.SpawnSoul(soulType - 1, spawner.transform.position + new Vector3(col, -row));
                    Soul currentSoul = soulObj.GetComponent<Soul>();
                    currentSoul.CurrentLocation = currentTile;
                    Tile.PlaceOnTile(currentSoul, currentTile);
                } else if (spiritType > LevelSyntax.NONE) {
                    // Exception in data #2-B
                    if (spiritType > Enum.GetNames(typeof(CustomEnums.SpiritType)).Length) {
                        throw new System.Exception("Invalid spiritType requested for during level construction;" +
                            " Encountered spiritType = [" + spiritType + "].");
                    }

                    int spiritLevel = (tilemapData[row][col] & LevelSyntax.MASK_SPIRIT_LEVEL) >> LevelSyntax.BITSHIFT_SPIRIT_LEVEL;

                    // We use value mappings of [0, 3] --> [1, 4] for the spirit level attribute.
                    if (spiritLevel >= Mergeable.SPIRIT_LEVEL_MAX) {
                        throw new System.Exception("Level of spirit object cannot be greater than " + Mergeable.SPIRIT_LEVEL_MAX + ";" +
                            " Encountered spiritLevel = [" + spiritLevel + "].");
                    } else {
                        GameObject spiritObj = spawner.SpawnSpirit(spiritType - 1, spiritLevel + 1, spawner.transform.position + new Vector3(col, -row));
                        Spirit currentSpirit = spiritObj.GetComponent<Spirit>();
                        currentSpirit.CurrentLocation = currentTile;
                        Tile.PlaceOnTile(currentSpirit, currentTile);
                    }
                }
            }
        }

        return data;
    }
}
