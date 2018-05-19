using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Soul : Mergeable {

    protected enum SoulType {
        ARAMITAMA, NIGIMITAMA, SAKIMITAMA, KUSHIMITAMA, NONE
    }
    protected SoulType soulType;

    public enum ColliderIndex {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }
    public List<Soul> neighbourSouls = new List<Soul>(Constants.NUM_NEIGHBOURS);


    protected override void Start () {
        base.Start();
        Debug.Assert(neighbourSouls.Count == Constants.NUM_NEIGHBOURS, "List of neighbours in Soul is of incorrect length.");
    }

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SOUL)) {
            string otherCollider = other.name;
            Soul s = other.gameObject.GetComponentInParent<Soul>();

            switch (otherCollider) {
                case Constants.COLLIDER_LEFT:
                    neighbourSouls[(int) ColliderIndex.RIGHT] = s;
                    break;

                case Constants.COLLIDER_RIGHT:
                    neighbourSouls[(int) ColliderIndex.LEFT] = s;
                    break;

                case Constants.COLLIDER_TOP:
                    neighbourSouls[(int) ColliderIndex.BOTTOM] = s;
                    break;

                case Constants.COLLIDER_BOTTOM:
                    neighbourSouls[(int) ColliderIndex.TOP] = s;
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown collider of Soul layer detected in OnTriggerEnter2D() for Soul script."));
                    break;
            }
        }
    }

    protected override void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SOUL)) {
            Soul s = other.gameObject.GetComponentInParent<Soul>();
            int soulIndex = neighbourSouls.IndexOf(s);
            if (soulIndex != -1) {
                neighbourSouls[soulIndex] = null;
            }
        }
    }

    protected override void OnRelease () {
        if (isActive) {
            if (HasMovedFromOldPosition()) {
                DisplaceObject();
                StartCoroutine(TriggerSoulMergeAttemptCoroutine());
            }
            
            SetObjectToInactiveState();
        }
    }

    protected virtual IEnumerator TriggerSoulMergeAttemptCoroutine () {
        // We introduce a delay to ensure that the list of neighbouring Souls are updated before we check for possible merging operation.
        yield return new WaitForSeconds(Constants.NEIGHBOUR_CHECK_DELAY);
        AttemptMerge();

        yield return new WaitForSeconds(Constants.TILE_TAINT_DELAY);
        tileManager.TakeMove();
    }

    /// <summary>
    /// Queries for all souls of same type that is connected to this soul.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>A list of all souls connected to this soul, which is of the same type.</returns>
    protected virtual List<Soul> QueryConnectedSouls (List<Soul> result, bool sameTypeRequired = true, SoulType requiredType = SoulType.NONE) {

        if (!result.Contains(this)) result.Add(this);

        Soul left = neighbourSouls[(int) ColliderIndex.LEFT];
        Soul right = neighbourSouls[(int) ColliderIndex.RIGHT];
        Soul top = neighbourSouls[(int) ColliderIndex.TOP];
        Soul bottom = neighbourSouls[(int) ColliderIndex.BOTTOM];

        if (left != null) {
            if (sameTypeRequired && left.soulType == requiredType) {
                if (!result.Contains(left)) result = left.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(left)) result = left.QueryConnectedSouls(result, false);
            }
        }

        if (right != null) {
            if (sameTypeRequired && right.soulType == requiredType) {
                if (!result.Contains(right)) result = right.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(right)) result = right.QueryConnectedSouls(result, false);
            }
        }

        if (top != null) {
           if (sameTypeRequired && top.soulType == requiredType) {
                if (!result.Contains(top)) result = top.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(top)) result = top.QueryConnectedSouls(result, false);
            }
        }

        if (bottom != null) {
            if (sameTypeRequired && bottom.soulType == requiredType) {
                if (!result.Contains(bottom)) result = bottom.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(bottom)) result = bottom.QueryConnectedSouls(result, false);
            }
        }

        return result;
    }

    /// <summary>
    /// Merges souls where appropriate and creates a corresponding resulting Spirit.
    /// Note that the Spirit of Harmony is created as a special case.
    /// </summary>
    protected override void AttemptMerge () {
        List<Soul> connectedSoulsOfSameType = QueryConnectedSouls(new List<Soul>(), requiredType: this.soulType);
        List<Soul> connectedSoulsOfAnyType = QueryConnectedSouls(new List<Soul>(), false);

        foreach (Soul t in connectedSoulsOfSameType) {
            Debug.Log(t);
        }
        
        int numSoulsAramitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.ARAMITAMA).Count;
        int numSoulsNigimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.NIGIMITAMA).Count;
        int numSoulsSakimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.SAKIMITAMA).Count;
        int numSoulsKushimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.KUSHIMITAMA).Count;

        if (Mathf.Max(numSoulsAramitama, numSoulsNigimitama, numSoulsSakimitama, numSoulsKushimitama) != 0) {
            if (numSoulsAramitama == numSoulsNigimitama &&
                numSoulsNigimitama == numSoulsSakimitama &&
                numSoulsSakimitama == numSoulsKushimitama) {
                foreach (Soul s in connectedSoulsOfAnyType) {
                    s.gameObject.SetActive(false);
                }

                SpawnSpiritOnMerge(specialCaseSatisfied: true);
                return;
            }
        }

        int numConnectedSoulsOfSameType = connectedSoulsOfSameType.Count;

        if (numConnectedSoulsOfSameType >= Constants.NUM_OBJECTS_FOR_MERGE) {
            foreach (Soul s in connectedSoulsOfSameType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(numConnectedSoulsOfSameType);
        }
    }

    protected virtual void SpawnSpiritOnMerge (int connectedSoulOfSameTypeCount = 0, bool specialCaseSatisfied = false) {
        if (!specialCaseSatisfied) {
            Debug.Assert(connectedSoulOfSameTypeCount >= Constants.NUM_OBJECTS_FOR_MERGE,
            "Soul-merging should not take place for less than " + Constants.NUM_OBJECTS_FOR_MERGE + " connected souls for non-special case.");
        }

        if (specialCaseSatisfied) {
            // Spawn a Spirit of Harmony
            Debug.Log("Spawning a Spirit of Harmony.");
        } else {
            // TODO: create/"create" the corresponding Spirit GameObject at current position.
            switch (soulType) {
                case SoulType.ARAMITAMA:
                    // Spawn a Spirit of Courage
                    Debug.Log("Spawning a Spirit of Courage.");
                    break;

                case SoulType.NIGIMITAMA:
                    // Spawn a Spirit of Friendship
                    Debug.Log("Spawning a Spirit of Friendship.");
                    break;

                case SoulType.SAKIMITAMA:
                    // Spawn a Spirit of Love
                    Debug.Log("Spawning a Spirit of Love.");
                    break;

                case SoulType.KUSHIMITAMA:
                    // Spawn a Spirit of Wisdom
                    Debug.Log("Spawning a Spirit of Wisdom.");
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown SoulType encountered by SpawnSpiritOnMerge() when non-special case is encountered."), this);
                    break;
            }
        }
    }

}
