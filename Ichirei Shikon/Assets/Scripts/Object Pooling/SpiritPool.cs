using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpiritPool : ObjectPool {

    /*
     * See Spirit class for indices used for accessing list of Spirit objects in object pool.
     * 
     * Prefab list indices convention:
     * [0 ~ 4]: Spirit of Courage - Levels 1 ~ 5
     * [5 ~ 9]: Spirit of Friendship - Levels 1 ~ 5
     * [10 ~ 14]: Spirit of Love - Levels 1 ~ 5
     * [15 ~ 19]: Spirit of Wisdom - Levels 1 ~ 5
     * [20 ~ 24]: Spirit of Harmony - Levels 1 ~ 5
     * 
     * Spirit Object Lists Convention:
     * [0]: Level 1 Spirit
     * [1]: Level 2 Spirit
     * [2]: Level 3 Spirit
     * [3]: Level 4 Spirit
     * [4]: Level 5 Spirit
     */

    // Each of these list of lists stores the same type of Spirit objects for all 5 levels.
    List<List<GameObject>> spiritCourageObjects = new List<List<GameObject>>();
    List<List<GameObject>> spiritFriendshipObjects = new List<List<GameObject>>();
    List<List<GameObject>> spiritLoveObjects = new List<List<GameObject>>();
    List<List<GameObject>> spiritWisdomObjects = new List<List<GameObject>>();
    List<List<GameObject>> spiritHarmonyObjects = new List<List<GameObject>>();

    // Use this for initialization
    protected override void Start () {
        AssertRequiredConditions();

        for (int i = 0; i < Constants.NUM_SPIRIT_LEVELS; i++) {
            GameObject spiritCourageObj = prefabs[(int) Spirit.SpiritType.COURAGE * Constants.NUM_SPIRIT_LEVELS + i];
            GameObject spiritFriendshipObj = prefabs[(int) Spirit.SpiritType.FRIENDSHIP * Constants.NUM_SPIRIT_LEVELS + i];
            GameObject spiritLoveObj = prefabs[(int) Spirit.SpiritType.LOVE * Constants.NUM_SPIRIT_LEVELS + i];
            GameObject spiritWisdomObj = prefabs[(int) Spirit.SpiritType.WISDOM * Constants.NUM_SPIRIT_LEVELS + i];
            GameObject spiritHarmonyObj = prefabs[(int) Spirit.SpiritType.HARMONY * Constants.NUM_SPIRIT_LEVELS + i];

            List<GameObject> spiritCourageObjList = new List<GameObject>();
            List<GameObject> spiritFriendshipObjList = new List<GameObject>();
            List<GameObject> spiritLoveObjList = new List<GameObject>();
            List<GameObject> spiritWisdomObjList = new List<GameObject>();
            List<GameObject> spiritHarmonyObjList = new List<GameObject>();

            for (int j = 0; j < quantity; j++) {
                for (int q = 0; q < quantity; q++) {
                    GameObject c = Instantiate(spiritCourageObj, parentTransform);
                    GameObject f = Instantiate(spiritFriendshipObj, parentTransform);
                    GameObject l = Instantiate(spiritLoveObj, parentTransform);
                    GameObject w = Instantiate(spiritWisdomObj, parentTransform);
                    GameObject h = Instantiate(spiritHarmonyObj, parentTransform);

                    c.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                    f.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                    l.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                    w.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state
                    h.SetActive(false);      // TODO: when finalized, uncheck the prefab's active state

                    spiritCourageObjList.Add(c);
                    spiritFriendshipObjList.Add(f);
                    spiritLoveObjList.Add(l);
                    spiritWisdomObjList.Add(w);
                    spiritHarmonyObjList.Add(h);
                }
            }

            spiritCourageObjects.Add(spiritCourageObjList);
            spiritFriendshipObjects.Add(spiritFriendshipObjList);
            spiritLoveObjects.Add(spiritLoveObjList);
            spiritWisdomObjects.Add(spiritWisdomObjList);
            spiritHarmonyObjects.Add(spiritHarmonyObjList);
        }
    }

    protected GameObject RetrieveSpirit (Spirit.SpiritType requiredType, int requiredLevel) {
        Debug.Assert(!Equals(requiredType, Spirit.SpiritType.NONE), "Invalid SpiritType requested for in SpiritPool.");
        Debug.Assert(requiredLevel > 0 && requiredLevel <= Constants.NUM_SPIRIT_LEVELS, "Invalid Spirit level requested for in SpiritPool");

        int index = requiredLevel;

        switch (requiredType) {
            case Spirit.SpiritType.COURAGE:
                for (int i = 0; i < quantity; i++) {
                    if (!spiritCourageObjects[index][i].activeSelf) {
                        return spiritCourageObjects[index][i];
                    }
                }
                break;

            case Spirit.SpiritType.FRIENDSHIP:
                for (int i = 0; i < quantity; i++) {
                    if (!spiritFriendshipObjects[index][i].activeSelf) {
                        return spiritFriendshipObjects[index][i];
                    }
                }
                break;

            case Spirit.SpiritType.LOVE:
                for (int i = 0; i < quantity; i++) {
                    if (!spiritLoveObjects[index][i].activeSelf) {
                        return spiritLoveObjects[index][i];
                    }
                }
                break;

            case Spirit.SpiritType.WISDOM:
                for (int i = 0; i < quantity; i++) {
                    if (!spiritWisdomObjects[index][i].activeSelf) {
                        return spiritWisdomObjects[index][i];
                    }
                }
                break;

            case Spirit.SpiritType.HARMONY:
                for (int i = 0; i < quantity; i++) {
                    if (!spiritHarmonyObjects[index][i].activeSelf) {
                        return spiritHarmonyObjects[index][i];
                    }
                }
                break;

            default:
                Debug.LogException(new System.Exception("Unknown SpirirtType encountered in RetrieveSpirit() in SpiritPool."));
                return null;
        }

        return null;
    }

    protected override void AssertRequiredConditions () {
        base.AssertRequiredConditions();

        // # of Spirit prefabs needed = NUM_SPIRIT_TYPES * NUM_SPIRIT_LEVELS
        Debug.Assert(prefabs.Length == (Constants.NUM_SPIRIT_TYPES * Constants.NUM_SPIRIT_LEVELS), "Incorrect number of prefabs detected in SpiritPool.");

        for (int i = 0; i < Constants.NUM_SPIRIT_LEVELS; i++) {
            Debug.Assert(prefabs[(int) Spirit.SpiritType.COURAGE * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritCourage>() != null, "SpiritOfCourage prefab should be in indices [0 ~ 4].");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.COURAGE * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritCourage>().Level == (i + 1), "Level of SpiritOfCourage prefab at index [" + (int) Spirit.SpiritType.COURAGE * Constants.NUM_SPIRIT_LEVELS + i + "] is incorrect.");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.FRIENDSHIP * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritFriendship>() != null, "SpiritOfFriendship prefab should be in indices [5 ~ 9].");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.FRIENDSHIP * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritFriendship>().Level == (i + 1), "Level of SpiritOfFriendship prefab at index [" + (int) Spirit.SpiritType.FRIENDSHIP * Constants.NUM_SPIRIT_LEVELS + i + "] is incorrect.");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.LOVE * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritLove>() != null, "SpiritOfLove prefab should be in indices [11 ~ 14].");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.LOVE * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritLove>().Level == (i + 1), "Level of SpiritOfLove prefab at index [" + (int) Spirit.SpiritType.LOVE * Constants.NUM_SPIRIT_LEVELS + i + "] is incorrect.");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.WISDOM * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritWisdom>() != null, "SpiritOfWisdom prefab should be in indices [15 ~ 19].");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.WISDOM * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritWisdom>().Level == (i + 1), "Level of SpiritOfWisdom prefab at index [" + (int) Spirit.SpiritType.WISDOM * Constants.NUM_SPIRIT_LEVELS + i + "] is incorrect.");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.HARMONY * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritHarmony>() != null, "SpiritOfHarmony prefab should be in indices [20 ~ 24].");
            Debug.Assert(prefabs[(int) Spirit.SpiritType.HARMONY * Constants.NUM_SPIRIT_LEVELS + i].GetComponent<SpiritHarmony>().Level == (i + 1), "Level of SpiritOfHarmony prefab at index [" + (int) Spirit.SpiritType.HARMONY * Constants.NUM_SPIRIT_LEVELS + i + "] is incorrect.");
        }
    }
}
