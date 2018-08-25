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
    public int SpiritLevel {
        get { return spiritLevel; }
    }

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

    public override void Taint() {
        base.Taint();
        GetComponent<ParticleSystem>().Play();
    }

    public override void Purify() {
        base.Purify();
        GetComponent<ParticleSystem>().Stop();
    }

    public virtual void ShowAreaOfEffect() {
        // Empty body, to be overriden by subclasses.
    }

    public virtual void HideAreaOfEffect() {
        // Empty body, to be overriden by subclasses.
    }

    public override void Merge() {
        base.Merge();
    }

    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = DetermineSpawnedSpiritLevel(mergedObjectCount, this.spiritLevel);
        Spirit spawnedSpirit = FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) spiritType, spiritLevel, triggeringObject.transform.position).GetComponent<Spirit>();
        spawnedSpirit.CurrentLocation = triggeringObject.CurrentLocation;
        Tile.PlaceOnTile(spawnedSpirit, triggeringObject.CurrentLocation);
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

    protected virtual void DegradeAfterTrigger() {
        // NOTE: Comment this body out to disable degrade-on-trigger feature.
        if (spiritLevel > 1) {
            spiritLevel--;
            GetComponent<SpriteRenderer>().sprite = sprites[spiritLevel - 1];
        } else {
            FindObjectOfType<ObjectSpawner>().RemoveObjectFromGame(gameObject);
        }

        // NOTE: uncomment this body out to disable degrade-on-trigger feature.
        // FindObjectOfType<ObjectSpawner>().RemoveObjectFromGame(gameObject);
    }
}
