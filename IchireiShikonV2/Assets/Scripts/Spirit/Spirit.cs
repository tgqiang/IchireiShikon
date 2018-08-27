using UnityEngine;

/// <summary>
/// <see cref="Spirit"/> objects have the basic power to purify <seealso cref="Tile"/>s, and different
/// <see cref="Spirit"/>s come with different additional abilities/quirks.
/// 
/// Tainted <see cref="Spirit"/>s, however, will not exercise any other effect besides tainting <seealso cref="Tile"/>s.
/// </summary>
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

    /// <summary>
    /// Since a single <see cref="Spirit"/> prefab is used as a template for spawning <see cref="Spirit"/>s,
    /// we have to initialize the spawned <see cref="Spirit"/>'s attributes correctly before it can be used in the game.
    /// </summary>
    /// <param name="level"></param>
    public virtual void InitializeSpirit(int level) {
        if (level <= 0) {
            throw new System.Exception("Level of spirit cannot be less than or equal to 0 upon initialization." +
                " Encountered level = [" + level + "].");
        }
        spiritLevel = level;
        GetComponent<SpriteRenderer>().sprite = sprites[spiritLevel - 1];
    }

    /// <summary>
    /// When <see cref="Spirit"/>s are tainted, they do not purify tiles
    /// (<seealso cref="Tile.Purify"/>) but taint (<seealso cref="Tile.Taint"/>) them instead.
    /// 
    /// Additionally, tainted <see cref="Spirit"/>s show a 'tainted' appearance that is played
    /// by a particle system in this <see cref="Spirit"/>'s object.
    /// </summary>
    public override void Taint() {
        base.Taint();
        GetComponent<ParticleSystem>().Play(false);
    }

    public override void Purify() {
        base.Purify();
        GetComponent<ParticleSystem>().Stop(false);
    }

    /// <summary>
    /// Implementations of this function are to be handled by subclasses of this class, since each type of <see cref="Spirit"/>'s AoE
    /// is inherently different from each other.
    /// </summary>
    public virtual void ShowAreaOfEffect() {
        // Empty body, to be overriden by subclasses.
    }

    /// <summary>
    /// Implementations of this function are to be handled by subclasses of this class, since each type of <see cref="Spirit"/>'s AoE
    /// is inherently different from each other.
    /// </summary>
    public virtual void HideAreaOfEffect() {
        // Empty body, to be overriden by subclasses.
    }

    /// <summary>
    /// See <see cref="Mergeable.SpawnObjectOnMerge(Mergeable, int)"/>.
    /// </summary>
    /// <param name="triggeringObject"></param>
    /// <param name="mergedObjectCount"></param>
    public override void SpawnObjectOnMerge(Mergeable triggeringObject, int mergedObjectCount) {
        int spiritLevel = DetermineSpawnedSpiritLevel(mergedObjectCount, this.spiritLevel);
        Spirit spawnedSpirit = FindObjectOfType<ObjectSpawner>().SpawnSpirit((int) spiritType, spiritLevel, triggeringObject.transform.position).GetComponent<Spirit>();
        spawnedSpirit.CurrentLocation = triggeringObject.CurrentLocation;
        Tile.PlaceOnTile(spawnedSpirit, triggeringObject.CurrentLocation);
    }

    /// <summary>
    /// See <see cref="Mergeable.IsSameTypeAs(Mergeable)"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if 'other' is of the same type as this one; False otherwise.</returns>
    public override bool IsSameTypeAs(Mergeable other) {
        if (other is Spirit) {
            return spiritType == (other as Spirit).spiritType && spiritLevel == (other as Spirit).spiritLevel;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Triggers this <see cref="Spirit"/>'s effect, when the player taps on it.
    /// </summary>
    public virtual void TriggerEffect() {
        // Empty body, to be overriden in subclasses.
    }

    /// <summary>
    /// An experimental feature, where <see cref="Spirit"/>s do not immediately disintegrate upon being consumed but reduce in power
    /// upon each usage. Only when they are consumed at <see cref="spiritLevel"/> 1 do they disappear upon being consumed.
    /// </summary>
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
