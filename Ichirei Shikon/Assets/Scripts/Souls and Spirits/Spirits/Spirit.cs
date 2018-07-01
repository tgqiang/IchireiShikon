using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Spirit : Mergeable {

    public enum SpiritType {
        COURAGE, FRIENDSHIP, LOVE, WISDOM, HARMONY, NONE
    }
    protected SpiritType spiritType;

    [SerializeField]
    protected int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]
    protected Sprite[] levelSprites;

    public List<Spirit> neighbourSpirits = new List<Spirit>(Configurable.NUM_NEIGHBOURS);

    protected override void Awake () {
        base.Awake();
        Debug.Assert(neighbourSpirits.Count == Configurable.NUM_NEIGHBOURS, "List of neighbours in Spirit is of incorrect length.");
        Debug.Assert(levelSprites != null, "Spirit level sprites not initialized in Spirit script.");
        Debug.Assert(levelSprites.Length == Configurable.instance.NUM_SPIRIT_LEVELS, "Spirit level sprites array in Spirit is of incorrect length.");
    }

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT])) {
            string otherCollider = other.name;
            Spirit s = other.gameObject.GetComponentInParent<Spirit>();

            switch (otherCollider) {
                case Configurable.COLLIDER_LEFT:
                    neighbourSpirits[(int) Configurable.ColliderIndex.RIGHT] = s;
                    break;

                case Configurable.COLLIDER_RIGHT:
                    neighbourSpirits[(int) Configurable.ColliderIndex.LEFT] = s;
                    break;

                case Configurable.COLLIDER_TOP:
                    neighbourSpirits[(int) Configurable.ColliderIndex.BOTTOM] = s;
                    break;

                case Configurable.COLLIDER_BOTTOM:
                    neighbourSpirits[(int) Configurable.ColliderIndex.TOP] = s;
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown collider of Spirit layer detected in OnTriggerEnter2D() for Spirit script."));
                    break;
            }
        }
    }

    protected override void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SPIRIT])) {
            Spirit s = other.gameObject.GetComponentInParent<Spirit>();
            RemoveSpiritFromNeighbourList(s);
        }
    }

    protected override void OnRelease () {
        if (isActive) {
            if (HasMovedFromOldPosition()) {
                DisplaceObject();
                StartCoroutine(TriggerSpiritMergeAttemptCoroutine());
            }

            // This should be called in subclasses instead, since there are more operations
            // to be handled by the subclasses while resolving events occuring during 'active' state.
            // SetObjectToInactiveState();
        }
    }

    public void RemoveSpiritFromNeighbourList (Spirit s) {
        int spiritIndex = neighbourSpirits.IndexOf(s);
        if (spiritIndex != -1) {
            neighbourSpirits[spiritIndex] = null;
        }
    }

    protected virtual IEnumerator TriggerSpiritMergeAttemptCoroutine () {
        // We introduce a delay to ensure that the list of neighbouring Spirits are updated before we check for possible merging operation.
        yield return new WaitForSeconds(Configurable.instance.NEIGHBOUR_CHECK_DELAY);
        AttemptMerge();

        //yield return new WaitForSeconds(Constants.TILE_TAINT_DELAY);
        tileManager.TakeMove();
    }

    protected static bool IsSameSpirit (Spirit s, SpiritType requiredType, int requiredLevel) {
        return IsSameType(s, requiredType) && IsSameLevel(s, requiredLevel);
    }

    protected static bool IsSameType (Spirit s, SpiritType requiredType) {
        return Equals(s.spiritType, requiredType);
    }

    protected static bool IsSameLevel(Spirit s, int requiredLevel) {
        return Equals(s.level, requiredLevel);
    }

    /// <summary>
    /// Queries for all spirits that are connected to this spirit, subject to desired
    /// constraints on whether the connected spirits must be of the same type as this one.
    /// 
    /// Note that spirits must share the same SpiritType and level to qualify as "same type".
    /// </summary>
    /// <param name="result">A List of Spirits, initialized to an empty list.</param>
    /// <param name="requiredLevel">The level of Spirit we should be looking out for, which should be the same level as this Spirit.</param>
    /// <param name="sameTypeRequired">A flag to indicate whether we want to filter out connected Spirits by same type or not.</param>
    /// <param name="requiredType">The type of Spirit required, if sameTypeRequired was set to true.</param>
    /// <returns>A list of all spirits connected to this spirit, subjected to whether the same type of spirit was required or not.</returns>
    protected virtual List<Spirit> QueryConnectedSpirits (List<Spirit> result, int requiredLevel, bool sameTypeRequired = true, SpiritType requiredType = SpiritType.NONE) {
        /*
         * If spirits are already at maximum level, they should not merge anymore ==> we have no need to do this query at all.
         * Note that level 5 spirits CANNOT be obtained from merging.
         */
        if (requiredLevel >= Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED) return result;

        if (!result.Contains(this)) result.Add(this);

        Spirit left = neighbourSpirits[(int) Configurable.ColliderIndex.LEFT];
        Spirit right = neighbourSpirits[(int) Configurable.ColliderIndex.RIGHT];
        Spirit top = neighbourSpirits[(int) Configurable.ColliderIndex.TOP];
        Spirit bottom = neighbourSpirits[(int) Configurable.ColliderIndex.BOTTOM];

        if (left != null) {
            if (sameTypeRequired && IsSameSpirit(left, requiredType, requiredLevel)) {
                if (!result.Contains(left)) result = left.QueryConnectedSpirits(result, requiredLevel, true, requiredType);
            } else if (!sameTypeRequired && IsSameLevel(left, requiredLevel)) {
                if (!result.Contains(left)) result = left.QueryConnectedSpirits(result, requiredLevel, false);
            }
        }

        if (right != null) {
            if (sameTypeRequired && IsSameSpirit(right, requiredType, requiredLevel)) {
                if (!result.Contains(right)) result = right.QueryConnectedSpirits(result, requiredLevel, true, requiredType);
            } else if (!sameTypeRequired && IsSameLevel(right, requiredLevel)) {
                if (!result.Contains(right)) result = right.QueryConnectedSpirits(result, requiredLevel, false);
            }
        }

        if (top != null) {
            if (sameTypeRequired && IsSameSpirit(top, requiredType, requiredLevel)) {
                if (!result.Contains(top)) result = top.QueryConnectedSpirits(result, requiredLevel, true, requiredType);
            } else if (!sameTypeRequired && IsSameLevel(top, requiredLevel)) {
                if (!result.Contains(top)) result = top.QueryConnectedSpirits(result, requiredLevel, false);
            }
        }

        if (bottom != null) {
            if (sameTypeRequired && IsSameSpirit(bottom, requiredType, requiredLevel)) {
                if (!result.Contains(bottom)) result = bottom.QueryConnectedSpirits(result, requiredLevel, true, requiredType);
            } else if (!sameTypeRequired && IsSameLevel(bottom, requiredLevel)) {
                if (!result.Contains(bottom)) result = bottom.QueryConnectedSpirits(result, requiredLevel, false);
            }
        }

        return result;
    }

    /// <summary>
    /// Merges souls where appropriate and creates a corresponding resulting Spirit.
    /// Note that the Spirit of Harmony is created as a special case.
    /// </summary>
    // NOTE: Merging mechanics for Spirits might be more complicated, do consider revisiting this function.
    public override void AttemptMerge () {
        List<Spirit> connectedSpiritsOfSameType = QueryConnectedSpirits(new List<Spirit>(), this.level, true, this.spiritType);
        List<Spirit> connectedSpiritsOfAnyType = QueryConnectedSpirits(new List<Spirit>(), this.level, false);

        int numSpiritsCourage = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.COURAGE).Count;
        int numSpiritsFriendship = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.FRIENDSHIP).Count;
        int numSpiritsLove = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.LOVE).Count;
        int numSpiritsWisdom = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.WISDOM).Count;
        int numSpiritsHarmony = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.HARMONY).Count;

        int minSpiritsOfAnyOneType = Mathf.Min(numSpiritsCourage, numSpiritsFriendship, numSpiritsLove, numSpiritsWisdom);
        int maxSpiritsOfAnyOneType = Mathf.Max(numSpiritsCourage, numSpiritsFriendship, numSpiritsLove, numSpiritsWisdom);

        if (minSpiritsOfAnyOneType == maxSpiritsOfAnyOneType && maxSpiritsOfAnyOneType != 0) {
            foreach (Spirit s in connectedSpiritsOfAnyType) {
                foreach (Spirit t in s.neighbourSpirits) {
                    if (t != null) t.RemoveSpiritFromNeighbourList(s);
                }
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(0, maxSpiritsOfAnyOneType, true);
            return;
        }

        int numConnectedSoulsOfSameType = connectedSpiritsOfSameType.Count;

        if (numConnectedSoulsOfSameType >= Configurable.instance.NUM_OBJECTS_FOR_MERGE) {
            foreach (Spirit s in connectedSpiritsOfSameType) {
                foreach (Spirit t in s.neighbourSpirits) {
                    if (t != null) t.RemoveSpiritFromNeighbourList(s);
                }
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(numConnectedSoulsOfSameType);
        }
    }

    // NOTE: Merging mechanics for Spirits might be more complicated, do consider revisiting this function.
    protected virtual void SpawnSpiritOnMerge (int connectedSpiritOfSameTypeCount = 0, int connectedSpiritOfAnyTypeCount = 0, bool specialCaseSatisfied = false) {
        if (!specialCaseSatisfied) {
            Debug.Assert(connectedSpiritOfSameTypeCount >= Configurable.instance.NUM_OBJECTS_FOR_MERGE,
            "Spirit-merging should not take place for less than " + Configurable.instance.NUM_OBJECTS_FOR_MERGE + " connected spirits for non-special case.");
        }

        if (specialCaseSatisfied) {
            int spawnedSpiritLevel = Mathf.Min(this.level + connectedSpiritOfAnyTypeCount, Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED);

            Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.HARMONY, spawnedSpiritLevel, this.transform.position).AttemptMerge();
        } else {
            int spawnedSpiritLevel = Mathf.Min(this.level + Mathf.FloorToInt((connectedSpiritOfSameTypeCount - 3) / 2), Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED);
            bool hasExtra = (connectedSpiritOfSameTypeCount - 3) % 2 == 1;

            switch (spiritType) {
                case SpiritType.COURAGE:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.COURAGE, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SpiritType.FRIENDSHIP:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.FRIENDSHIP, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SpiritType.LOVE:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.LOVE, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SpiritType.WISDOM:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.WISDOM, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SpiritType.HARMONY:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.HARMONY, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown SpiritType encountered by SpawnSpiritOnMerge() when non-special case is encountered."), this);
                    break;
            }

            if (hasExtra || connectedSpiritOfSameTypeCount >= (10 - 2 * this.level)) {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.nearestTilePosition.x, this.nearestTilePosition.y), Vector2.zero, 5f, LayerMask.GetMask(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]));

                if (hit) {
                    Tile currentTile = hit.collider.GetComponent<Tile>();
                    currentTile.Purify();
                }
            }
        }
    }

    public virtual void IncrementSpiritLevel(bool hasReceivedSpecialBuff = false) {
        /*
         * Spirit of Friendship/Harmony levels are capped at MAX_SPIRIT_LEVEL_UNBUFFED (currently at 4).
         * Reason for doing this is to prevent possibly-infinite level-buffing exploitation.
         */
        if (this.level == Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED &&
            Equals(this.spiritType, SpiritType.FRIENDSHIP) || Equals(this.spiritType, SpiritType.HARMONY)) {
            return;
        }

        if (hasReceivedSpecialBuff && this.level < Configurable.instance.MAX_SPIRIT_LEVEL_BUFFED) {
            RaiseSpiritLevelOnce();
        } else if (this.level < Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED) {
            RaiseSpiritLevelOnce();
        }
    }

    protected virtual void RaiseSpiritLevelOnce () {
        this.level += 1;
        spriteRenderer.sprite = levelSprites[this.level - 1];
    }

    public virtual void SetLevel (int desiredLevel) {
        this.level = desiredLevel;
        spriteRenderer.sprite = levelSprites[this.level - 1];
    }

    protected virtual void TriggerEffect () {
        // Spirits will trigger their effects through their corresponding SpiritEffect objects.
        // Effect-triggering algorithm is implemented in each different typed-Spirit subclasses.
    }

}
