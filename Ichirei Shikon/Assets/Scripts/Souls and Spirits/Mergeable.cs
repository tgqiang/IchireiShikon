using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Mergeable : MonoBehaviour {

    public static TileManager tileManager;
    public static TileHighlighter tileHighlighter;
    
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// A flag for determining whether this object is the target of player input or not.
    /// </summary>
    protected bool isActive;

    // We need this attribute to ensure that this object can either move to a free tile or to its own position.
    protected BoxCollider2D colliderSelf;

    /// <summary>
    /// Records the previous position of this soul,
    /// which will be used to assist in object-snapping and checking if a turn has been used.
    /// </summary>
    protected Vector3 prevPosition;
    protected Vector3 nearestTilePosition;

    /// <summary>
    /// A flag for tracking if this object has been displaced, which is used for extra handling in subclasses where required.
    /// </summary>
    protected bool wasDisplaced;

    protected float timeSinceInput;     // for tracking whether player input is a "tap" or a "touch-and-drag"


    protected virtual void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer component is missing for a GameObject containing the Mergeable script.");

        colliderSelf = GetComponent<BoxCollider2D>();
        Debug.Assert(colliderSelf != null, "BoxCollider 2D component is missing for a GameObject containing the Mergeable script.");

        prevPosition = transform.position;
        nearestTilePosition = transform.position;

        Debug.Assert(tileHighlighter != null, "Tile Highlighter GameObject/Component is missing, likely not set by TileHighlighter script. Also check if TileHighlighter Game Object exists in scene.");
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (Input.GetMouseButtonDown(Constants.MOUSE_BUTTON_LEFT)) {
            // we only want to allow the object, that was clicked on by the player, to be movable.
            isActive = IsClickedOn(Input.mousePosition);
        }

        if (Input.GetMouseButton(Constants.MOUSE_BUTTON_LEFT)) {
            timeSinceInput += Time.deltaTime;
            OnGrab();
        }

        if (Input.GetMouseButtonUp(Constants.MOUSE_BUTTON_LEFT)) {
            OnRelease();
        }
    }

    protected virtual void OnTriggerEnter2D (Collider2D other) {
        // Empty-bodied function for subclasses to override.
    }

    protected virtual void OnTriggerExit2D (Collider2D other) {
        // Empty-bodied function for subclasses to override.
    }

    protected virtual bool IsClickedOn (Vector3 mousePosition) {
        Vector3 pointInWorldSpace = Constants.mainCamera.ScreenToWorldPoint(mousePosition);
        return GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(pointInWorldSpace.x, pointInWorldSpace.y));
    }

    protected virtual void OnGrab () {
        if (isActive) {
            HighlightCurrentTileSpot();
        }
    }

    protected virtual void HighlightCurrentTileSpot () {
        Vector3 worldPoint = Constants.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // TODO: adjust this for mobile version.
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPoint.x, worldPoint.y), Vector2.zero, Mathf.Infinity, Constants.desiredRaycastLayers);

        // If raycast hits a collider with a Tile component, that Tile is unoccupied.
        if (hit) {
            if (hit.collider.name == Constants.LAYER_NAME_TILE_BOUNDS || hit.collider == colliderSelf) {
                tileHighlighter.Highlight(hit.transform.position);
                nearestTilePosition = hit.transform.position;
            }
        }
        spriteRenderer.color = Constants.colorActive;
    }

    protected virtual void OnRelease () {
        CheckPlayerInputType();

        if (isActive) {
            if (HasMovedFromOldPosition()) {
                DisplaceObject();
                tileManager.TakeMove();
            }

            SetObjectToInactiveState();
        }

        timeSinceInput = 0;
    }

    protected virtual void CheckPlayerInputType () {
        if (timeSinceInput <= Constants.INPUT_DIFFERENTIATION_THRESHOLD) {
            Debug.Log("Tap registered.");
        } else {
            Debug.Log("Touch-and-drag registered.");
        }
    }

    protected virtual bool HasMovedFromOldPosition () {
        wasDisplaced = nearestTilePosition.x != prevPosition.x || nearestTilePosition.y != prevPosition.y;
        return wasDisplaced;
    }

    protected virtual void DisplaceObject () {
        nearestTilePosition.z = Constants.MERGEABLE_OBJECTS_Z_OFFSET;
        prevPosition = nearestTilePosition;
    }

    protected virtual void SetObjectToInactiveState () {
        isActive = false;

        gameObject.transform.position = prevPosition;
        spriteRenderer.color = Constants.colorInactive;
        tileHighlighter.Unhighlight();

        timeSinceInput = 0f;
    }

    protected virtual void AttemptMerge () {
        // Empty-bodied function for subclasses to override.
        // TODO: implement merging algorithm in subclasses.
    }
}
