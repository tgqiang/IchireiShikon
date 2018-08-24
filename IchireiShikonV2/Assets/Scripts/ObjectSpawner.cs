using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public GameObject[] soulPrefabs;
    public GameObject[] spiritPrefabs;

    void Awake () {
        if (tilePrefabs.Length != Enum.GetNames(typeof(CustomEnums.TileType)).Length) {
            throw new System.Exception("List of tile prefabs have incorrect number of elements.");
        }

        if (soulPrefabs.Length != Enum.GetNames(typeof(CustomEnums.SoulType)).Length) {
            throw new System.Exception("List of soul prefabs have incorrect number of elements.");
        }

        if (spiritPrefabs.Length != Enum.GetNames(typeof(CustomEnums.SpiritType)).Length) {
            throw new System.Exception("List of spirit prefabs have incorrect number of elements.");
        }
    }

    /// <summary>
    /// Spawns a tile at a desired position, given an index corresponding to the desired type of tile to spawn,
    /// and the desired spawn position.
    /// </summary>
    /// <param name="tileType">The index of the desired tile to spawn.</param>
    /// <param name="position"></param>
    /// <returns>The tile GameObject that was spawned.</returns>
    public GameObject SpawnTile(int tileType, Vector3 position) {
        return Spawner.CreateSpawn(tilePrefabs[tileType], position, Quaternion.Euler(Vector3.zero));
    }

    /// <summary>
    /// Spawns a soul at a desired position, given an index corresponding to the desired type of soul to spawn,
    /// and the desired spawn position.
    /// </summary>
    /// <param name="soulType">The index of the desired soul to spawn.</param>
    /// <param name="position"></param>
    /// <returns>The soul GameObject that was spawned.</returns>
    public GameObject SpawnSoul(int soulType, Vector3 position) {
        return Spawner.CreateSpawn(soulPrefabs[soulType], position, Quaternion.Euler(Vector3.zero));
    }

    /// <summary>
    /// Spawns a spirit at a desired position, given an index corresponding to the desired type of spirit to spawn,
    /// its level, and the desired spawn position.
    /// </summary>
    /// <param name="spiritType">The index of the desired spirit to spawn.</param>
    /// <param name="spiritLevel">The desired level of the spirit to spawn, which is 1-indexed.</param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject SpawnSpirit(int spiritType, int spiritLevel, Vector3 position) {
        GameObject spiritObject = Spawner.CreateSpawn(spiritPrefabs[spiritType], position, Quaternion.Euler(Vector3.zero));
        spiritObject.GetComponent<Spirit>().InitializeSpirit(spiritLevel);
        return spiritObject;
    }

    public void RemoveObjectFromGame(GameObject obj) {
        Spawner.Destroy(obj);
    }
}
