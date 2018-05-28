using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Constants : MonoBehaviour {

    // ======================================= System Configurations ======================================= //
    public const int MOUSE_BUTTON_LEFT = 0;
    public const float INPUT_DIFFERENTIATION_THRESHOLD = .4f;

    public const int NUM_SOUL_OBJECTS = 4;
    public const int NUM_SPIRIT_LEVELS = 5;
    public const int NUM_SPIRIT_TYPES = 5;
    public const int MIN_OBJECT_POOL_QUANTITY = 20;

    public const string OBJECT_POOL_NAME = "ObjectPool";
    public static SoulPool soulPool;
    public static SpiritPool spiritPool;
    // ========================================================================================================== //

    
    
    // ====================================== Tile System Configurations ======================================= //
    public const int NUM_TILE_SPRITES = 3;
    public const string UNTAINTED_TILE_NAME = "Tile_Untainted";
    public const string SHIELDED_TILE_NAME = "Tile_Invulnerable";
    public const string TAINTED_TILE_NAME = "Tile_Tainted";

    public const int NUM_NEIGHBOURS = 4;
    // ========================================================================================================== //

    
    
    // ======================================= Game Logic Configurations ======================================== //
    public const float NEIGHBOUR_CHECK_DELAY = 0.1f;
    public const float TILE_TAINT_DELAY = 0.2f;

    public const int NUM_OBJECTS_FOR_MERGE = 3;

    public const int MIN_SPIRIT_LEVEL = 1;
    public const int MAX_SPIRIT_LEVEL_UNBUFFED = 4;
    public const int MAX_SPIRIT_LEVEL_BUFFED = 5;

    public static int[] SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL = {2, 4, 6, 8, 8};
    // ========================================================================================================== //

    
    
    // ==================================== Collision-related Configurations ==================================== //
    public enum ColliderIndex {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }

    /*
     * These layer names are stored here, as they will be required for many important operations,
     * namely raycasting for vacant tile and Soul/Spirit neighbour checking via collision triggers.
     */
    public const string LAYER_NAME_DEFAULT = "Default";
    public const string LAYER_NAME_SOUL = "Soul";
    public const string LAYER_NAME_SOUL_BOUNDS = "SoulBounds";
    public const string LAYER_NAME_SPIRIT = "Spirit";
    public const string LAYER_NAME_SPIRIT_BOUNDS = "SpiritBounds";
    public const string LAYER_NAME_TILE = "Tile";
    public const string LAYER_NAME_TILE_BOUNDS = "TileBounds";
    public const string LAYER_NAME_TAINTED_TILE_COLLIDER = "TaintedTileCollider";

    /*
     * These constants represent the 4 colliders used to detect neighbouring Souls/Spirits.
     * These constants *SHOULD NOT BE CHANGED* unless the Soul/Spirit prefab's core structure has been changed.
     */
    public const string COLLIDER_LEFT = "LeftCollider";
    public const string COLLIDER_RIGHT = "RightCollider";
    public const string COLLIDER_TOP = "TopCollider";
    public const string COLLIDER_BOTTOM = "BottomCollider";

    /// <summary>
    /// This constant is used to "bring" the Soul/Spirit object in front,
    /// so that they will be detected first during raycasting for vacant tile.
    /// </summary>
    public const float MERGEABLE_OBJECTS_Z_OFFSET = -1f;

    /// <summary>
    /// This constant is used to reduce the size of the spirit-effect collider so that it does not come into contact
    /// with the neighbouring tiles beyond the effect zone.
    /// </summary>
    public const float COLLIDER_SCALE_OFFSET = 0.35f;

    // Raycasts are used for detecting the tile that an object should snap to,
    // and should only check for these collision layers.
    public static LayerMask desiredRaycastLayers;
    // ========================================================================================================== //


    // ======================================= Game Sprite Configurations ======================================= //
    /// <summary> Color of the sprite when it is being acted on by the player input. </summary> ///
    public static Color colorActive = new Color(1, 1, 1, 0.5f);
    /// <summary> Color of the sprite when it is not being acted on by the player input. </summary> ///
    public static Color colorInactive = Color.white;
    // ========================================================================================================== //

    
    
    // TODO: Possible future use for having movable game camera //
    public static Camera mainCamera;


    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "Main Camera is missing from game Scene.");

        GameObject objectPool = GameObject.Find(OBJECT_POOL_NAME);
        Debug.Assert(objectPool != null, "ObjectPool game object is missing from game Scene.");

        soulPool = objectPool.GetComponent<SoulPool>();
        Debug.Assert(soulPool != null, "SoulPool script is missing from game Scene.");

        spiritPool = objectPool.GetComponent<SpiritPool>();
        Debug.Assert(soulPool != null, "SpiritPool script is missing from game Scene.");

        desiredRaycastLayers = LayerMask.GetMask(LAYER_NAME_TILE_BOUNDS, LAYER_NAME_SOUL_BOUNDS, LAYER_NAME_SPIRIT_BOUNDS);
    }

    public void ResetMainCamera () {
        mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "Main Camera was previously present in game Scene but is now missing.");
    }
}
