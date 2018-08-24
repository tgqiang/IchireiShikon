using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : Mergeable {

    protected CustomEnums.SoulType soulType;

	public virtual void Start () {
        if (sprites.Length != Enum.GetNames(typeof(CustomEnums.SoulType)).Length) {
            throw new System.Exception("List of soul sprites have incorrect length.");
        }

        if (currentLocation == null) {
            throw new System.Exception("Location attributes are missing for spawned soul object.");
        }
	}

    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = Mathf.Min(1 + Mathf.FloorToInt((mergedObjectCount - 3) / 2), SPIRIT_LEVEL_MAX);
        FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) soulType, spiritLevel, triggeringObject.transform.position);
    }

    public override bool IsSameTypeAs(Mergeable other) {
        if (other is Soul) {
            return soulType == (other as Soul).soulType;
        } else {
            return false;
        }
    }
}
