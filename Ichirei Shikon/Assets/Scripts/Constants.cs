using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Constants : MonoBehaviour {

    /* Game Input Configurations */
    public const int MOUSE_BUTTON_LEFT = 0;

    /* Game Conventions Configurations */
    public const int NUM_TILE_SPRITES = 3;
    public const string UNTAINTED_TILE_NAME = "Tile_Untainted";
    public const string SHIELDED_TILE_NAME = "Tile_Invulnerable";
    public const string TAINTED_TILE_NAME = "Tile_Tainted";

    public const int NUM_NEIGHBOURS = 4;
    public const int NUM_OBJECTS_FOR_MERGE = 3;

    /* Collision Related Configurations */
    public const string LAYER_NAME_DEFAULT = "Default";
    public const string LAYER_NAME_SOUL = "Soul";
    public const string LAYER_NAME_SPIRIT = "Spirit";
    public const string LAYER_NAME_TILE = "Tile";
    public const string LAYER_NAME_TILE_BOUNDS = "TileBounds";

    public const string COLLIDER_LEFT = "LeftCollider";
    public const string COLLIDER_RIGHT = "RightCollider";
    public const string COLLIDER_TOP = "TopCollider";
    public const string COLLIDER_BOTTOM = "BottomCollider";

    public const float MERGEABLE_OBJECTS_Z_OFFSET = -1f;

    public const float NEIGHBOUR_CHECK_DELAY = 0.1f;
    public const float TILE_TAINT_DELAY = 0.2f;

    // Raycasts are used for detecting the tile that an object should snap to,
    // and should ignore these collision layers.
    public static LayerMask desiredRaycastLayers;

    /* Game Sprite Configurations */
    /// <summary> Color of the sprite when it is being acted on by the player input. </summary> ///
    public static Color colorActive = new Color(1, 1, 1, 0.5f);
    /// <summary> Color of the sprite when it is not being acted on by the player input. </summary> ///
    public static Color colorInactive = Color.white;

    /* Game Camera Configurations */
    public static Camera mainCamera;


    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "Main Camera is missing from game Scene.");

        desiredRaycastLayers = LayerMask.GetMask(LAYER_NAME_TILE_BOUNDS, LAYER_NAME_SOUL, LAYER_NAME_SPIRIT);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetMainCamera () {
        mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "Main Camera was previously present in game Scene but is now missing.");
    }
}
