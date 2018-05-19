using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : Mergeable {

    protected enum SpiritType {
        COURAGE, FRIENDSHIP, LOVE, WISDOM, HARMONY
    }
    protected SpiritType spiritType;

    [SerializeField]
    protected int level;

    protected enum ColliderIndex {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }
    protected List<Spirit> neighbourSpirits = new List<Spirit>(Constants.NUM_NEIGHBOURS);

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_NAME_SPIRIT)) {
            string otherCollider = other.name;
            Spirit s = other.gameObject.GetComponentInParent<Spirit>();

            switch (otherCollider) {
                case Constants.COLLIDER_LEFT:
                    neighbourSpirits[(int) ColliderIndex.RIGHT] = s;
                    break;

                case Constants.COLLIDER_RIGHT:
                    neighbourSpirits[(int) ColliderIndex.LEFT] = s;
                    break;

                case Constants.COLLIDER_TOP:
                    neighbourSpirits[(int) ColliderIndex.BOTTOM] = s;
                    break;

                case Constants.COLLIDER_BOTTOM:
                    neighbourSpirits[(int) ColliderIndex.TOP] = s;
                    break;

                default:
                    Debug.Log("No neighbours detected.");
                    break;
            }

            foreach (Spirit n in neighbourSpirits) {
                int firstIndex = neighbourSpirits.IndexOf(n);
                int lastIndex = neighbourSpirits.LastIndexOf(n);

                Debug.Assert(firstIndex == lastIndex, "Bug in neighbour-check for Spirit script: one Spirit appears as a neighbour of this spirit more than once.");
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
        base.OnRelease();
        if (!HasMovedFromOldPosition()) {
            //Merge();
        }
    }

    /// <summary>
    /// <para>
    /// Queries 2 sets of required data: (A) All spirits of same type that are connected to this spirit, and
    /// (B) Spirits of any type that are connected to this spirit.
    /// </para>
    /// <para>
    /// Case (B) is also required since we also want to check if a special case, the Spirit of Harmony, can be spawned.
    /// Note that the Spirit of Harmony can be formed from a composition of EQUAL amounts of all types of spirits.
    /// </para>
    /// </summary>
    /// <param name="resultForSameSpiritType"> A result list for storing connected souls of same type. </param>
    /// <param name="resultForAnySpiritType"> A result list for storing connected souls of any type. </param>
    protected virtual void QueryConnectedSpirits (out List<Spirit> resultForSameSpiritType, out List<Spirit> resultForAnySpiritType) {
        resultForSameSpiritType = new List<Spirit>();
        resultForAnySpiritType = new List<Spirit>();

        Spirit left = neighbourSpirits[(int) ColliderIndex.LEFT];
        Spirit right = neighbourSpirits[(int) ColliderIndex.RIGHT];
        Spirit top = neighbourSpirits[(int) ColliderIndex.TOP];
        Spirit bottom = neighbourSpirits[(int) ColliderIndex.BOTTOM];

        if (left != null) {
            if (!resultForAnySpiritType.Contains(left)) resultForAnySpiritType.Add(left);

            if (left.spiritType == this.spiritType) {
                if (!resultForSameSpiritType.Contains(left)) resultForSameSpiritType.Add(left);
                left.QueryConnectedSpirits(out resultForSameSpiritType, out resultForAnySpiritType);
            }
        }

        if (right != null) {
            if (!resultForAnySpiritType.Contains(right)) resultForAnySpiritType.Add(right);

            if (right.spiritType == this.spiritType) {
                if (!resultForSameSpiritType.Contains(right)) resultForSameSpiritType.Add(right);
                right.QueryConnectedSpirits(out resultForSameSpiritType, out resultForAnySpiritType);
            }
        }

        if (top != null) {
            if (!resultForAnySpiritType.Contains(top)) resultForAnySpiritType.Add(top);

            if (top.spiritType == this.spiritType) {
                if (!resultForSameSpiritType.Contains(top)) resultForSameSpiritType.Add(top);
                top.QueryConnectedSpirits(out resultForSameSpiritType, out resultForAnySpiritType);
            }
        }

        if (bottom != null) {
            if (!resultForAnySpiritType.Contains(bottom)) resultForAnySpiritType.Add(bottom);

            if (bottom.spiritType == this.spiritType) {
                if (!resultForSameSpiritType.Contains(bottom)) resultForSameSpiritType.Add(bottom);
                bottom.QueryConnectedSpirits(out resultForSameSpiritType, out resultForAnySpiritType);
            }
        }
    }

    /// <summary>
    /// Merges souls where appropriate and creates a corresponding resulting Spirit.
    /// Note that the Spirit of Harmony is created as a special case.
    /// </summary>
    protected override void AttemptMerge () {
        List<Spirit> connectedSpiritsOfSameType = new List<Spirit>();
        List<Spirit> connectedSpiritsOfAnyType = new List<Spirit>();
        QueryConnectedSpirits(out connectedSpiritsOfSameType, out connectedSpiritsOfAnyType);

        int numSpiritsCourage = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.COURAGE).Count;
        int numSpiritsFriendship = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.FRIENDSHIP).Count;
        int numSpiritsLove = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.LOVE).Count;
        int numSpiritsWisdom = connectedSpiritsOfAnyType.FindAll(s => s.spiritType == SpiritType.WISDOM).Count;

        if (numSpiritsCourage == numSpiritsFriendship &&
            numSpiritsFriendship == numSpiritsLove &&
            numSpiritsLove == numSpiritsWisdom) {
            foreach (Spirit s in connectedSpiritsOfAnyType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(specialCaseSatisfied: true);
            return;
        }

        int numConnectedSpiritsOfSameType = connectedSpiritsOfSameType.Count;

        if (numConnectedSpiritsOfSameType >= Constants.NUM_OBJECTS_FOR_MERGE) {
            foreach (Spirit s in connectedSpiritsOfSameType) {
                s.gameObject.SetActive(false);
            }

            SpawnSpiritOnMerge(numConnectedSpiritsOfSameType);
        }
    }

    protected virtual void SpawnSpiritOnMerge (int connectedSpiritOfSameTypeCount = 0, bool specialCaseSatisfied = false) {
        Debug.Assert(!specialCaseSatisfied && connectedSpiritOfSameTypeCount >= Constants.NUM_OBJECTS_FOR_MERGE,
            "Soul-merging should not take place for less than " + Constants.NUM_OBJECTS_FOR_MERGE + " connected souls for non-special case.");

        if (specialCaseSatisfied) {
            // Spawn a Spirit of Harmony
        } else {
            // TODO: create/"create" the corresponding Spirit GameObject at current position.
            switch (spiritType) {
                case SpiritType.COURAGE:
                    // Spawn a Spirit of Courage
                    break;

                case SpiritType.FRIENDSHIP:
                    // Spawn a Spirit of Friendship
                    break;

                case SpiritType.LOVE:
                    // Spawn a Spirit of Love
                    break;

                case SpiritType.WISDOM:
                    // Spawn a Spirit of Wisdom
                    break;

                case SpiritType.HARMONY:
                    // Spawn a Spirit of Harmony
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
