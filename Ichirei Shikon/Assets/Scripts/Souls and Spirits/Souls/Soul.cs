﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Soul : Mergeable {

    public enum SoulType {
        ARAMITAMA, NIGIMITAMA, SAKIMITAMA, KUSHIMITAMA, NONE
    }
    protected SoulType soulType;

    public List<Soul> neighbourSouls = new List<Soul>(Configurable.NUM_NEIGHBOURS);


    protected override void Awake () {
        base.Awake();
        Debug.Assert(neighbourSouls.Count == Configurable.NUM_NEIGHBOURS, "List of neighbours in Soul is of incorrect length.");
    }

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SOUL])) {
            string otherCollider = other.name;
            Soul s = other.gameObject.GetComponentInParent<Soul>();

            switch (otherCollider) {
                case Configurable.COLLIDER_LEFT:
                    neighbourSouls[(int) Configurable.ColliderIndex.RIGHT] = s;
                    break;

                case Configurable.COLLIDER_RIGHT:
                    neighbourSouls[(int) Configurable.ColliderIndex.LEFT] = s;
                    break;

                case Configurable.COLLIDER_TOP:
                    neighbourSouls[(int) Configurable.ColliderIndex.BOTTOM] = s;
                    break;

                case Configurable.COLLIDER_BOTTOM:
                    neighbourSouls[(int) Configurable.ColliderIndex.TOP] = s;
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown collider of Soul layer detected in OnTriggerEnter2D() for Soul script."));
                    break;
            }
        }
    }

    protected override void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.SOUL])) {
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
        yield return new WaitForSeconds(Configurable.instance.NEIGHBOUR_CHECK_DELAY);
        AttemptMerge();
        tileManager.TakeMove();     // NOTE: investigate why putting this call as another coroutine does not trigger this call.
    }

    /// <summary>
    /// Queries for all souls that are connected to this soul, subject to desired
    /// constraints on whether the connected souls must be of the same type as this one.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>A list of all souls connected to this soul, subjected to whether the same type of soul was required or not.</returns>
    protected virtual List<Soul> QueryConnectedSouls (List<Soul> result, bool sameTypeRequired = true, SoulType requiredType = SoulType.NONE) {

        if (!result.Contains(this)) result.Add(this);

        Soul left = neighbourSouls[(int) Configurable.ColliderIndex.LEFT];
        Soul right = neighbourSouls[(int) Configurable.ColliderIndex.RIGHT];
        Soul top = neighbourSouls[(int) Configurable.ColliderIndex.TOP];
        Soul bottom = neighbourSouls[(int) Configurable.ColliderIndex.BOTTOM];

        if (left != null) {
            if (sameTypeRequired && Equals(left.soulType, requiredType)) {
                if (!result.Contains(left)) result = left.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(left)) result = left.QueryConnectedSouls(result, false);
            }
        }

        if (right != null) {
            if (sameTypeRequired && Equals(right.soulType, requiredType)) {
                if (!result.Contains(right)) result = right.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(right)) result = right.QueryConnectedSouls(result, false);
            }
        }

        if (top != null) {
           if (sameTypeRequired && Equals(top.soulType, requiredType)) {
                if (!result.Contains(top)) result = top.QueryConnectedSouls(result, true, requiredType);
            } else if (!sameTypeRequired) {
                if (!result.Contains(top)) result = top.QueryConnectedSouls(result, false);
            }
        }

        if (bottom != null) {
            if (sameTypeRequired && Equals(bottom.soulType, requiredType)) {
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
    public override void AttemptMerge () {
        List<Soul> connectedSoulsOfSameType = QueryConnectedSouls(new List<Soul>(), requiredType: this.soulType);
        List<Soul> connectedSoulsOfAnyType = QueryConnectedSouls(new List<Soul>(), false);
        
        int numSoulsAramitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.ARAMITAMA).Count;
        int numSoulsNigimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.NIGIMITAMA).Count;
        int numSoulsSakimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.SAKIMITAMA).Count;
        int numSoulsKushimitama = connectedSoulsOfAnyType.FindAll(s => s.soulType == SoulType.KUSHIMITAMA).Count;

        int minSoulsOfAnyOneType = Mathf.Min(numSoulsAramitama, numSoulsNigimitama, numSoulsSakimitama, numSoulsKushimitama);
        int maxSoulsOfAnyOneType = Mathf.Max(numSoulsAramitama, numSoulsNigimitama, numSoulsSakimitama, numSoulsKushimitama);

        if (minSoulsOfAnyOneType == maxSoulsOfAnyOneType && maxSoulsOfAnyOneType != 0) {
            foreach (Soul s in connectedSoulsOfAnyType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(0, maxSoulsOfAnyOneType, true);
            return;
        }

        int numConnectedSoulsOfSameType = connectedSoulsOfSameType.Count;

        if (numConnectedSoulsOfSameType >= Configurable.instance.NUM_OBJECTS_FOR_MERGE) {
            foreach (Soul s in connectedSoulsOfSameType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(numConnectedSoulsOfSameType);
        }
    }

    protected virtual void SpawnSpiritOnMerge (int connectedSoulOfSameTypeCount = 0, int connectedSoulOfAnyTypeCount = 0, bool specialCaseSatisfied = false) {
        if (!specialCaseSatisfied) {
            Debug.Assert(connectedSoulOfSameTypeCount >= Configurable.instance.NUM_OBJECTS_FOR_MERGE,
            "Soul-merging should not take place for less than " + Configurable.instance.NUM_OBJECTS_FOR_MERGE + " connected souls for non-special case.");
        }

        if (specialCaseSatisfied) {
            int spawnedSpiritLevel = Mathf.Min(connectedSoulOfAnyTypeCount, Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED);

            Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.HARMONY, spawnedSpiritLevel, this.transform.position).AttemptMerge();
        } else {
            int spawnedSpiritLevel = Mathf.Min(1 + Mathf.FloorToInt((connectedSoulOfSameTypeCount - 3) / 2), Configurable.instance.MAX_SPIRIT_LEVEL_UNBUFFED);
            bool hasExtra = (connectedSoulOfSameTypeCount - 3) % 2 == 1;

            switch (soulType) {
                case SoulType.ARAMITAMA:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.COURAGE, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SoulType.NIGIMITAMA:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.FRIENDSHIP, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SoulType.SAKIMITAMA:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.LOVE, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                case SoulType.KUSHIMITAMA:
                    Configurable.instance.spiritPool.SpawnSpirit(Spirit.SpiritType.WISDOM, spawnedSpiritLevel, this.transform.position).AttemptMerge();
                    break;

                default:
                    Debug.LogException(new System.Exception("Unknown SoulType encountered by SpawnSpiritOnMerge() when non-special case is encountered."), this);
                    break;
            }

            if (hasExtra || connectedSoulOfSameTypeCount >= 10) {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.nearestTilePosition.x, this.nearestTilePosition.y), Vector2.zero, 5f, LayerMask.GetMask(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE]));

                if (hit) {
                    Tile currentTile = hit.collider.GetComponent<Tile>();
                    currentTile.Purify();
                }
            }
        }
    }

}
