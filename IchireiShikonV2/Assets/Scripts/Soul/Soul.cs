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

    public override void Merge() {
        base.Merge();
    }

    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = Mathf.Min(Mathf.FloorToInt((mergedObjectCount - 1) / 2), SPIRIT_LEVEL_MAX);
        Spirit spawnedSpirit = FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) soulType, spiritLevel, triggeringObject.transform.position).GetComponent<Spirit>();
        spawnedSpirit.SetLocation(triggeringObject.GetLocation());
        Tile.PlaceOnTile(spawnedSpirit, triggeringObject.GetLocation());
    }

    public override bool IsSameTypeAs(Mergeable other) {
        if (other is Soul) {
            return soulType == (other as Soul).soulType;
        } else {
            return false;
        }
    }
}
