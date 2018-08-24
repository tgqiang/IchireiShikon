using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : Mergeable {

    protected CustomEnums.SpiritType spiritType;
    /// <summary>
    /// The level of the spirit, which is 1-indexed.
    /// </summary>
    [SerializeField]
    protected int spiritLevel;

    public virtual void Start() {
        if (sprites.Length != Mergeable.SPIRIT_LEVEL_MAX) {
            throw new System.Exception("List of spirit sprites have incorrect length.");
        }

        if (currentLocation == null) {
            throw new System.Exception("Location attributes are missing for spawned spirit object.");
        }
    }

    public virtual void InitializeSpirit(int level) {
        if (level <= 0) {
            throw new System.Exception("Level of spirit cannot be less than or equal to 0 upon initialization." +
                " Encountered level = [" + level + "].");
        }
        spiritLevel = level;
        GetComponent<SpriteRenderer>().sprite = sprites[spiritLevel - 1];
    }

    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = Mathf.Min(this.spiritLevel + Mathf.FloorToInt((mergedObjectCount - 3) / 2), SPIRIT_LEVEL_MAX);
        FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) spiritType, spiritLevel, triggeringObject.transform.position);
    }

    public override bool IsSameTypeAs(Mergeable other) {
        if (other is Spirit) {
            return spiritType == (other as Spirit).spiritType && spiritLevel == (other as Spirit).spiritLevel;
        } else {
            return false;
        }
    }

    public virtual void TriggerEffect() {
        // Empty body, to be overriden in subclasses.
    }
}
