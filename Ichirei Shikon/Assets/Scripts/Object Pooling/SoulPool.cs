using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoulPool : ObjectPool {

    /* See Soul class for indices used for accessing list of Soul objects in object pool. */

    List<List<GameObject>> soulObjects = new List<List<GameObject>>();

	// Use this for initialization
	protected override void Start () {
        AssertRequiredConditions();

        for (int i = 0; i < prefabs.Length; i++) {
            GameObject soulObj = prefabs[i];
            List<GameObject> soulObjList = new List<GameObject>();

            for (int j = 0; j < quantity; j++) {
                GameObject o = Instantiate(soulObj, parentTransform);
                o.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                soulObjList.Add(o);
            }

            soulObjects.Add(soulObjList);
        }
	}

    protected GameObject RetrieveSoul (Soul.SoulType requiredType) {
        Debug.Assert(!Equals(requiredType, Soul.SoulType.NONE), "Invalid SoulType requested for in SoulPool.");
        return RetrieveSoul((int) requiredType);
    }

    protected GameObject RetrieveSoul (int requiredType) {
        Debug.Assert(requiredType < (int) Soul.SoulType.NONE, "Invalid integer 'requiredType' received in RetrieveSoul for SoulPool.");
        int index = requiredType;

        for (int i = 0; i < quantity; i++) {
            if (!soulObjects[index][i].activeSelf) {
                return soulObjects[index][i];
            }
        }

        return null;
    }

    /// <summary>
    /// Spawns a random type of Soul in the Scene.
    /// 
    /// Note that the caller of this method MUST check if the spot where the Soul should be spawned is VACANT before calling this.
    /// </summary>
    /// <param name="desiredPosition">Where the Soul should be spawned, which typically is a vacant tile's position.</param>
    public void SpawnRandomSoul (Vector3 desiredPosition) {
        // { ALL SOULTYPES } \ { NONE }
        int soulType = Random.Range((int) Soul.SoulType.ARAMITAMA, (int) Soul.SoulType.NONE);
        GameObject soulObj = RetrieveSoul(soulType);
        SpawnSoulObjectInScene(desiredPosition, soulObj);
    }

    /// <summary>
    /// Finds a Soul object that can be used from the object pool, and spawns it in the game Scene.
    /// 
    /// Note that the caller of this method MUST check if the spot where the Soul should be spawned is VACANT before calling this.
    /// </summary>
    /// <param name="requiredType">The desired type of Soul to spawn in the Scene.</param>
    /// <param name="desiredPosition">Where the Soul should be spawned, which typically is a vacant tile's position.</param>
    public void SpawnSoul (Soul.SoulType requiredType, Vector3 desiredPosition) {
        GameObject soulObj = RetrieveSoul(requiredType);
        SpawnSoulObjectInScene(desiredPosition, soulObj);
    }

    private static void SpawnSoulObjectInScene (Vector3 desiredPosition, GameObject soulObj) {
        soulObj.transform.position = desiredPosition;
        soulObj.SetActive(true);
    }

    protected override void AssertRequiredConditions () {
        base.AssertRequiredConditions();

        Debug.Assert(prefabs.Length == Configurable.NUM_SOUL_OBJECTS, "Incorrect number of prefabs detected in SoulPool.");
        Debug.Assert(prefabs[(int) Soul.SoulType.ARAMITAMA].GetComponent<SoulAramitama>() != null, "SoulAramitama prefab should be in [0]th index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.NIGIMITAMA].GetComponent<SoulNigimitama>() != null, "SoulNigimitama prefab should be in [1]st index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.SAKIMITAMA].GetComponent<SoulSakimitama>() != null, "SoulSakimitama prefab should be in [2]nd index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.KUSHIMITAMA].GetComponent<SoulKushimitama>() != null, "SoulKushimitama prefab should be in [3]rd index.");
    }
}
