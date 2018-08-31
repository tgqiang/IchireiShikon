/// <summary>
/// This class contains the data format declarations of each tile unit of a level's tile-map.
/// </summary>
/// 
/// 10 bits are used to represent 4 attributes of possible objects occupying a unit in a level's Tile Map.
/// 
/// _ _ | _ _ _ | _ _ _ | _ _
/// 9 8   7 6 5   4 3 2   1 0
/// 
/// + Bits 0~1 represent 4 types of tiles, order can be obtained at CustomEnums.TileType. A tile always exist unless denoted with EMPTY_UNIT.
/// + Bits 2~4 represent 4 types of souls, order can be obtained at CustomEnums.SoulType. '0' is reserved for no-Soul-object attribute.
/// + Bits 5~7 represent 4 types of spirits, order can be obtained at CustomEnums.SpiritType. '0' is reserved for no-Spirit-object attribute.
/// + Bit 8~9 represent the level of a spirit, if it exists at that unit. This maps from levels [0,3] in 0-index to levels [1,4] in 1-index.
/// + '-1' is used if nothing exists at that unit.
public class LevelSyntax {

    public const int LEVEL_TILEMAP_MAX_LENGTH = 17;
    public const int LEVEL_TILEMAP_MAX_HEIGHT = 9;

    /// <summary>
    /// Denotes that nothing should be present at the current tile unit.
    /// </summary>
    public const int EMPTY_UNIT = -1;
    /// <summary>
    /// A bitmask to extract the 2 least significant bits for the type of tile to be spawned at current tile unit.
    /// </summary>
    public const int MASK_TILE = 3;
    /// <summary>
    /// A bitmask to extract the next 2 least significant bits for the type of soul to be spawned at current tile unit.
    /// </summary>
    public const int MASK_SOUL = 28;
    public const int BITSHIFT_SOUL = 2;
    /// <summary>
    /// A bitmask to extract the next 2 most significant bits for the type of spirit to be spawned at current tile unit.
    /// </summary>
    public const int MASK_SPIRIT = 224;
    public const int BITSHIFT_SPIRIT = 5;
    /// <summary>
    /// A bitmask to extract the most significant bit for the level of the spirit to be spawned at current tile unit.
    /// 
    /// This is provided that a spirit object is to be spawned there.
    /// </summary>
    public const int MASK_SPIRIT_LEVEL = 768;
    public const int BITSHIFT_SPIRIT_LEVEL = 8;
    /// <summary>
    /// Denotes that no Mergeable object is to be spawned at current tile unit.
    /// </summary>
    public const int NONE = 0;
    public const int MAX_NUMERICAL_REPRESENTATION = 1 << 10;
}
