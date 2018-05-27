using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFriendship : Spirit {

    public static SpiritOfFriendshipEffect spiritFriendshipEffect;

    protected override void Awake () {
        base.Awake();
        spiritType = SpiritType.FRIENDSHIP;

        spiritFriendshipEffect = GameObject.Find(SpiritOfFriendshipEffect.NAME).GetComponent<SpiritOfFriendshipEffect>();
        Debug.Assert(spiritFriendshipEffect != null, "SpiritOfFriendshipEffect GameObject/Component is missing. Also check if SpiritOfFriendshipEffect.NAME attribute matches the spirit effect game object.");
    }

    protected override void HighlightCurrentTileSpot () {
        Vector3 worldPoint = Constants.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // TODO: adjust this for mobile version.
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPoint.x, worldPoint.y), Vector2.zero, Mathf.Infinity, Constants.desiredRaycastLayers);

        // If raycast hits a collider with a Tile component, that Tile is unoccupied.
        if (hit) {
            if (hit.collider.name == Constants.LAYER_NAME_TILE_BOUNDS || hit.collider == colliderSelf) {
                tileHighlighter.Highlight(hit.transform.position);
                spiritFriendshipEffect.Highlight(hit.transform.position, this.level);
                nearestTilePosition = hit.transform.position;
            }
        }

        spriteRenderer.color = Constants.colorActive;
    }

    protected override void OnRelease () {
        base.OnRelease();

        spiritFriendshipEffect.Unhighlight();

        if (timeSinceInput <= Constants.INPUT_DIFFERENTIATION_THRESHOLD) {
            TriggerEffect();
        }

        SetObjectToInactiveState();
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

    protected override void TriggerEffect () {
        if (isActive) {
            SetObjectToInactiveState();
            spiritFriendshipEffect.CastEffect(this.level);
            gameObject.SetActive(false);
        }
    }

}
