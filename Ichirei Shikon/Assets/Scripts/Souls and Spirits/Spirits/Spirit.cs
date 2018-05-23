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

    public List<Spirit> neighbourSpirits = new List<Spirit>(Constants.NUM_NEIGHBOURS);

    protected override void Start () {
        base.Start();
        Debug.Assert(neighbourSpirits.Count == Constants.NUM_NEIGHBOURS, "List of neighbours in Spirit is of incorrect length.");
    }

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT)) {
            string otherCollider = other.name;
            Spirit s = other.gameObject.GetComponentInParent<Spirit>();

            switch (otherCollider) {
                case Constants.COLLIDER_LEFT:
                    neighbourSpirits[(int) Constants.ColliderIndex.RIGHT] = s;
                    break;

                case Constants.COLLIDER_RIGHT:
                    neighbourSpirits[(int) Constants.ColliderIndex.LEFT] = s;
                    break;

                case Constants.COLLIDER_TOP:
                    neighbourSpirits[(int) Constants.ColliderIndex.BOTTOM] = s;
                    break;

                case Constants.COLLIDER_BOTTOM:
                    neighbourSpirits[(int) Constants.ColliderIndex.TOP] = s;
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown collider of Spirit layer detected in OnTriggerEnter2D() for Spirit script."));
                    break;
            }
        }
    }

    protected override void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT)) {
            Spirit s = other.gameObject.GetComponentInParent<Spirit>();
            int spiritIndex = neighbourSpirits.IndexOf(s);
            if (spiritIndex != -1) {
                neighbourSpirits[spiritIndex] = null;
            }
        }
    }

    protected override void OnRelease () {
        if (isActive) {
            if (HasMovedFromOldPosition()) {
                DisplaceObject();
                StartCoroutine(TriggerSpiritMergeAttemptCoroutine());
            }

            SetObjectToInactiveState();
        }
    }

    protected virtual IEnumerator TriggerSpiritMergeAttemptCoroutine () {
        // We introduce a delay to ensure that the list of neighbouring Spirits are updated before we check for possible merging operation.
        yield return new WaitForSeconds(Constants.NEIGHBOUR_CHECK_DELAY);
        AttemptMerge();

        yield return new WaitForSeconds(Constants.TILE_TAINT_DELAY);
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
    /// <param name="result"></param>
    /// <returns>A list of all spirits connected to this spirit, subjected to whether the same type of spirit was required or not.</returns>
    protected virtual List<Spirit> QueryConnectedSpirits (List<Spirit> result, int requiredLevel, bool sameTypeRequired = true, SpiritType requiredType = SpiritType.NONE) {
        // If spirits are already at maximum level, they should not merge anymore.
        // This implies we have no need to do this query at all.
        if (requiredLevel >= Constants.MAX_SPIRIT_LEVEL_UNBUFFED) return result;

        if (!result.Contains(this)) result.Add(this);

        Spirit left = neighbourSpirits[(int) Constants.ColliderIndex.LEFT];
        Spirit right = neighbourSpirits[(int) Constants.ColliderIndex.RIGHT];
        Spirit top = neighbourSpirits[(int) Constants.ColliderIndex.TOP];
        Spirit bottom = neighbourSpirits[(int) Constants.ColliderIndex.BOTTOM];

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
    // TODO: Merging mechanics for Spirits might be more complicated, do consider revisiting this function.
    protected override void AttemptMerge () {
        List<Spirit> connectedSpiritsOfSameType = QueryConnectedSpirits(new List<Spirit>(), this.level, true, this.spiritType);
        List<Spirit> connectedSpiritsOfAnyType = QueryConnectedSpirits(new List<Spirit>(), this.level, false);

        foreach (Spirit t in connectedSpiritsOfSameType) {
            Debug.Log(t);
        }

        int numSpiritsCourage = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.COURAGE).Count;
        int numSpiritsFriendship = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.FRIENDSHIP).Count;
        int numSpiritsLove = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.LOVE).Count;
        int numSpiritsWisdom = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.WISDOM).Count;
        int numSpiritsHarmony = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.HARMONY).Count;

        if (Mathf.Max(numSpiritsCourage, numSpiritsFriendship, numSpiritsLove, numSpiritsWisdom) != 0) {
            if (numSpiritsCourage == numSpiritsFriendship &&
                numSpiritsFriendship == numSpiritsLove &&
                numSpiritsLove == numSpiritsWisdom) {
                foreach (Spirit s in connectedSpiritsOfAnyType) {
                    s.gameObject.SetActive(false);
                }

                SpawnSpiritOnMerge(specialCaseSatisfied: true);
                return;
            }
        }

        int numConnectedSoulsOfSameType = connectedSpiritsOfSameType.Count;

        if (numConnectedSoulsOfSameType >= Constants.NUM_OBJECTS_FOR_MERGE) {
            foreach (Spirit s in connectedSpiritsOfSameType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(numConnectedSoulsOfSameType);
        }
    }

    // TODO: Merging mechanics for Spirits might be more complicated, do consider revisiting this function.
    // TODO: Need to also make sure to increment the level of the spawned spirit.
    protected virtual void SpawnSpiritOnMerge (int connectedSoulOfSameTypeCount = 0, bool specialCaseSatisfied = false) {
        if (!specialCaseSatisfied) {
            Debug.Assert(connectedSoulOfSameTypeCount >= Constants.NUM_OBJECTS_FOR_MERGE,
            "Spirit-merging should not take place for less than " + Constants.NUM_OBJECTS_FOR_MERGE + " connected spirits for non-special case.");
        }

        if (specialCaseSatisfied) {
            // Spawn a Spirit of Harmony
            Debug.Log("Spawning a Spirit of Harmony.");
        } else {
            // TODO: create/"create" the corresponding Spirit GameObject at current position.
            switch (spiritType) {
                case SpiritType.COURAGE:
                    // Spawn a Spirit of Courage
                    Debug.Log("Spawning a Spirit of Courage.");
                    break;

                case SpiritType.FRIENDSHIP:
                    // Spawn a Spirit of Friendship
                    Debug.Log("Spawning a Spirit of Friendship.");
                    break;

                case SpiritType.LOVE:
                    // Spawn a Spirit of Love
                    Debug.Log("Spawning a Spirit of Love.");
                    break;

                case SpiritType.WISDOM:
                    // Spawn a Spirit of Wisdom
                    Debug.Log("Spawning a Spirit of Wisdom.");
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown SpiritType encountered by SpawnSpiritOnMerge() when non-special case is encountered."), this);
                    break;
            }
        }
    }

    protected virtual void TriggerEffect () {
        // Empty-bodied function for subclasses to override.
        // TODO: implement trigger-effect algorithm in subclasses.
    }

}
