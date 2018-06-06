using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Configurable : MonoBehaviour {

    public static Configurable instance;

    // ======================================= System Configurations ======================================= //
    // ******************** [CONSTANTS (SHOULD NOT BE ALTERED)] ******************** //
    public const int MOUSE_BUTTON_LEFT = 0;

    public const int NUM_SOUL_OBJECTS = 4;
    public const int NUM_SPIRIT_TYPES = 5;

    // ************************* [CONFIGURABLE ATTRIBUTES] ************************* //
    public float INPUT_DIFFERENTIATION_THRESHOLD = .4f;

    public int NUM_SPIRIT_LEVELS = 5;
    public int MIN_OBJECT_POOL_QUANTITY = 20;

    public SoulPool soulPool;
    public SpiritPool spiritPool;
    // ========================================================================================================== //


    // ====================================== Tile System Configurations ======================================= //
    // ******************** [CONSTANTS (SHOULD NOT BE ALTERED)] ******************** //
    public const int NUM_TILE_SPRITES = 4;
    public const int NUM_NEIGHBOURS = 4;

    // ************************* [CONFIGURABLE ATTRIBUTES] ************************* //
    public string NEUTRAL_TILE_NAME = "Tile_Neutral";
    public string PURIFIED_TILE_NAME = "Tile_Purified";
    public string SHIELDED_TILE_NAME = "Tile_Invulnerable";
    public string TAINTED_TILE_NAME = "Tile_Tainted";
    // ========================================================================================================== //



    // ======================================= Game Logic Configurations ======================================== //
    // ******************** [CONSTANTS (SHOULD NOT BE ALTERED)] ******************** //
    public const int MIN_SPIRIT_LEVEL = 1;

    // ************************* [CONFIGURABLE ATTRIBUTES] ************************* //
    public float NEIGHBOUR_CHECK_DELAY = 0.1f;
    public float TILE_TAINT_DELAY = 0.2f;

    public int NUM_OBJECTS_FOR_MERGE = 3;

    public int MAX_SPIRIT_LEVEL_UNBUFFED = 4;
    public int MAX_SPIRIT_LEVEL_BUFFED = 5;

    public int[] SPIRIT_OF_LOVE_SOULS_SPAWNED_AT_LEVEL = { 2, 4, 6, 8, 8 };
    public int[] SPIRIT_OF_HARMONY_SOULS_SPAWNED_AT_LEVEL = { 2, 4, 6, 8, 8 };
    // ========================================================================================================== //



    // ==================================== Collision-related Configurations ==================================== //
    // ************************* [CONFIGURABLE ATTRIBUTES] ************************* //
    public enum ColliderIndex {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }

    /// <summary>
    /// An array of layer names, used namely for raycasting for vacant tile and Soul/Spirit neighbour checking via collision triggers.
    /// </summary>
    public string[] LAYER_NAMES;
    /// <summary>
    /// An enum of layer names that stores the respective indices for accessing LAYER_NAMES attribute.
    /// Note that this should MATCH the layer settings.
    /// </summary>
    public enum LayerNameIndices {
        DEFAULT,
        TILE,
        TILE_BOUNDS,
        TAINTED_TILE_COLLIDER,
        SOUL,
        SOUL_BOUNDS,
        SPIRIT,
        SPIRIT_BOUNDS
    }
    public int NUM_COLLISION_LAYERS_USED = 8;

    /*
     * These constants represent the 4 colliders used to detect neighbouring Souls/Spirits.
     * These are set as constants due to the use of 'switch' statements for neighbour-detection handling.
     * These constants *SHOULD NOT BE CHANGED* unless the Soul/Spirit prefab's core structure has been changed.
     */
    public const string COLLIDER_LEFT = "LeftCollider";
    public const string COLLIDER_RIGHT = "RightCollider";
    public const string COLLIDER_TOP = "TopCollider";
    public const string COLLIDER_BOTTOM = "BottomCollider";

    /// <summary>
    /// This attribute is used to "bring" the Soul/Spirit object in front,
    /// so that they will be detected first during raycasting for vacant tile.
    /// </summary>
    public float MERGEABLE_OBJECTS_Z_OFFSET = -1f;

    /// <summary>
    /// This attribute is used to reduce the size of the spirit-effect collider so that it does not come into contact
    /// with the neighbouring tiles beyond the effect zone.
    /// </summary>
    public float COLLIDER_SCALE_OFFSET = 0.35f;

    // Raycasts are used for detecting the tile that an object should snap to,
    // and should only check for these collision layers.
    public LayerMask desiredRaycastLayers;
    // ========================================================================================================== //


    // ======================================= Game Sprite Configurations ======================================= //
    /// <summary> Color of the sprite when it is being acted on by the player input. </summary> ///
    public Color colorActive = new Color(1, 1, 1, 0.5f);
    /// <summary> Color of the sprite when it is not being acted on by the player input. </summary> ///
    public Color colorInactive = Color.white;
    // ========================================================================================================== //


    // ======================================= Game Enhancements Configurations ======================================= //
    /// <summary> Color of the camera background when all tiles are tainted. </summary> ///
    public Color colorGameOver = Color.black;
    /// <summary> Color of the camera background when all tiles are purified. </summary> ///
    public Color colorVictory = new Color(0, 155f / 255f, 1);
    // ========================================================================================================== //



    // NOTE: Possible future use for having movable game camera //
    public Camera mainCamera;


    void Awake () {
        Debug.Assert(mainCamera != null, "Main Camera is missing from game Scene.");
        Debug.Assert(soulPool != null, "SoulPool script is missing from game Scene.");
        Debug.Assert(spiritPool != null, "SpiritPool script is missing from game Scene.");

        instance = this;
    }

}
