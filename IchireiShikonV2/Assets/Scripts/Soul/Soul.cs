using System;

/// <summary>
/// <see cref="Soul"/>s are the building blocks of <seealso cref="Spirit"/>s. They must be merged in quantities to form
/// <seealso cref="Spirit"/>s of different <seealso cref="Spirit.spiritType"/>s and <seealso cref="Spirit.spiritLevel"/>s.
/// </summary>
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

    /// <summary>
    /// When a soul object is tainted, it is instantly destroyed.
    /// </summary>
    public override void Taint() {
        Tile.RemoveFromTileAndDestroy(this, currentLocation);
    }

    /// <summary>
    /// See <see cref="Mergeable.SpawnObjectOnMerge(Mergeable, int)"/>.
    /// </summary>
    /// <param name="triggeringObject"></param>
    /// <param name="mergedObjectCount"></param>
    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = DetermineSpawnedSpiritLevel(mergedObjectCount);
        Spirit spawnedSpirit = FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) soulType, spiritLevel, triggeringObject.transform.position).GetComponent<Spirit>();
        spawnedSpirit.CurrentLocation = triggeringObject.CurrentLocation;
        Tile.PlaceOnTile(spawnedSpirit, triggeringObject.CurrentLocation);
    }

    /// <summary>
    /// See <see cref="Mergeable.IsSameTypeAs(Mergeable)"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if 'other' is of the same type as this one; False otherwise.</returns>
    public override bool IsSameTypeAs(Mergeable other) {
        if (other is Soul) {
            return soulType == (other as Soul).soulType;
        } else {
            return false;
        }
    }
}
