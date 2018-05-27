using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritCourage : Spirit {

    public static SpiritOfCourageEffect spiritCourageEffect;

    protected override void Awake () {
        base.Awake();
        spiritType = SpiritType.COURAGE;

        spiritCourageEffect = GameObject.Find(SpiritOfCourageEffect.NAME).GetComponent<SpiritOfCourageEffect>();
        Debug.Assert(spiritCourageEffect != null, "SpiritOfCourageEffect GameObject/Component is missing. Also check if SpiritOfCourageEffect.NAME attribute matches the spirit effect game object.");
    }

    protected override void HighlightCurrentTileSpot () {
        Vector3 worldPoint = Constants.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // TODO: adjust this for mobile version.
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPoint.x, worldPoint.y), Vector2.zero, Mathf.Infinity, Constants.desiredRaycastLayers);

        // If raycast hits a collider with a Tile component, that Tile is unoccupied.
        if (hit) {
            if (hit.collider.name == Constants.LAYER_NAME_TILE_BOUNDS || hit.collider == colliderSelf) {
                tileHighlighter.Highlight(hit.transform.position);
                spiritCourageEffect.Highlight(hit.transform.position, this.level);
                nearestTilePosition = hit.transform.position;
            }
        }

        spriteRenderer.color = Constants.colorActive;
    }

    protected override void OnRelease () {
        base.OnRelease();

        spiritCourageEffect.Unhighlight();
        
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
            spiritCourageEffect.CastEffect();
            gameObject.SetActive(false);
        }
    }

}
