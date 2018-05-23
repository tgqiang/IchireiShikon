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
                GameObject o = Instantiate(soulObj);
                o.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                soulObjList.Add(o);
            }

            soulObjects.Add(soulObjList);
        }
	}

    protected GameObject RetrieveSoul (Soul.SoulType requiredType) {
        Debug.Assert(!Equals(requiredType, Soul.SoulType.NONE), "Invalid SoulType requested for in SoulPool.");
        int index = (int) requiredType;

        for (int i = 0; i < quantity; i++) {
            if (!soulObjects[index][i].activeSelf) {
                return soulObjects[index][i];
            }
        }

        return null;
    }

    protected override void AssertRequiredConditions () {
        base.AssertRequiredConditions();

        Debug.Assert(prefabs.Length == Constants.NUM_SOUL_OBJECTS, "Incorrect number of prefabs detected in SoulPool.");
        Debug.Assert(prefabs[(int) Soul.SoulType.ARAMITAMA].GetComponent<SoulAramitama>() != null, "SoulAramitama prefab should be in [0]th index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.NIGIMITAMA].GetComponent<SoulNigimitama>() != null, "SoulNigimitama prefab should be in [1]st index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.SAKIMITAMA].GetComponent<SoulSakimitama>() != null, "SoulSakimitama prefab should be in [2]nd index.");
        Debug.Assert(prefabs[(int) Soul.SoulType.KUSHIMITAMA].GetComponent<SoulKushimitama>() != null, "SoulKushimitama prefab should be in [3]rd index.");
    }
}
