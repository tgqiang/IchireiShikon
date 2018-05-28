using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritLove : Spirit {

    public static SpiritOfLoveEffect spiritLoveEffect;

    protected override void Awake () {
        base.Awake();
        spiritType = SpiritType.LOVE;

        spiritLoveEffect = FindObjectOfType<SpiritOfLoveEffect>();
        Debug.Assert(spiritLoveEffect != null, "SpiritOfLoveEffect GameObject/Component is missing. Also check if SpiritOfLoveEffect.NAME attribute matches the spirit effect game object.");
    }

    protected override void HighlightCurrentTileSpot () {
        Vector3 worldPoint = Configurable.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // TODO: adjust this for mobile version.
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPoint.x, worldPoint.y), Vector2.zero, Mathf.Infinity, Configurable.instance.desiredRaycastLayers);

        // If raycast hits a collider with a Tile component, that Tile is unoccupied.
        if (hit) {
            if (hit.collider.name == Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.TILE_BOUNDS] || hit.collider == colliderSelf) {
                tileHighlighter.Highlight(hit.transform.position);
                spiritLoveEffect.Highlight(hit.transform.position, this.level);
                nearestTilePosition = hit.transform.position;
            }
        }

        spriteRenderer.color = Configurable.instance.colorActive;
    }

    protected override void OnRelease () {
        base.OnRelease();

        spiritLoveEffect.Unhighlight();

        if (timeSinceInput <= Configurable.instance.INPUT_DIFFERENTIATION_THRESHOLD) {
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
            spiritLoveEffect.CastEffect();
            gameObject.SetActive(false);
        }
    }

}
