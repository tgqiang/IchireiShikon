using UnityEngine;

public class LevelBuilderTileUnit : MonoBehaviour {

    static LevelBuilderBackend levelBuilderBackend;

    [Header("This tile's coordinates, in [row, column]")]
    public Vector2Int tileCoordinates;

    [Header("Tile at current unit")]
    public bool isTilePresent;
    private bool isTilePresentOldValue;
    public CustomEnums.TileType tileType;

    [Header("Soul at current unit")]
    public bool isSoulPresent;
    private bool isSoulPresentOldValue;
    public CustomEnums.SoulType soulType;

    [Header("Spirit at current unit")]
    public bool isSpiritPresent;
    private bool isSpiritPresentOldValue;
    public int spiritLevel;
    public CustomEnums.SpiritType spiritType;

    [Header("Tile references")]
    public SpriteRenderer tileRenderer;
    public Sprite[] tileSprites;

    [Header("Soul references")]
    public SpriteRenderer soulRenderer;
    public Sprite[] soulSprites;

    [Header("Spirit references")]
    public SpriteRenderer spiritRenderer;
    public Sprite[] spiritSprites;

    bool hasChanged;

    void Start() {
        levelBuilderBackend = FindObjectOfType<LevelBuilderBackend>();
        UpdateValueAtCurrentTile();
    }

    // Update is called once per frame
    void Update () {
        // If 'isTilePresent' was toggled...
        if (isTilePresent != isTilePresentOldValue) {
            isTilePresentOldValue = isTilePresent;
            hasChanged = true;
        }

        if (isTilePresentOldValue) {
            tileRenderer.enabled = true;
            tileRenderer.sprite = tileSprites[(int) tileType];

            soulRenderer.sprite = soulSprites[(int) soulType];

            if (spiritLevel <= 0) spiritLevel = 1;
            else if (spiritLevel > Mergeable.SPIRIT_LEVEL_MAX) spiritLevel = Mergeable.SPIRIT_LEVEL_MAX;
            spiritRenderer.sprite = spiritSprites[(int) spiritType * Mergeable.SPIRIT_LEVEL_MAX + (spiritLevel - 1)];

            // If 'isSoulPresent' checkbox was toggled...
            if (isSoulPresent != isSoulPresentOldValue) {
                isSoulPresentOldValue = isSoulPresent;
                hasChanged = true;

                if (isSoulPresentOldValue) {
                    // ... show the soul if desired...
                    soulRenderer.enabled = true;

                    // ... and hide the spirit.
                    isSpiritPresent = isSpiritPresentOldValue = false;
                    spiritRenderer.enabled = false;
                } else {
                    // ... hide the soul.
                    soulRenderer.enabled = false;
                }
            }
            // If 'isSpiritPresent' checkbox was toggled...
            else if (isSpiritPresent != isSpiritPresentOldValue) {
                isSpiritPresentOldValue = isSpiritPresent;
                hasChanged = true;

                if (isSpiritPresentOldValue) {
                    // ... show the spirit if desired...
                    spiritRenderer.enabled = true;

                    // ... and hide the soul.
                    isSoulPresent = isSoulPresentOldValue = false;
                    soulRenderer.enabled = false;
                } else {
                    // ... hide the spirit.
                    spiritRenderer.enabled = false;
                }
            }
        } else {
            tileRenderer.enabled = false;

            isSoulPresent = false;
            soulRenderer.enabled = false;

            isSpiritPresent = false;
            spiritRenderer.enabled = false;
        }

        if (hasChanged) {
            UpdateValueAtCurrentTile();
            hasChanged = false;
        }
	}

    void UpdateValueAtCurrentTile() {
        int value = LevelSyntax.EMPTY_UNIT;

        if (isTilePresentOldValue) {
            value = ((int) tileType);

            if (isSoulPresentOldValue) {
                value += (((int) soulType + 1) << LevelSyntax.BITSHIFT_SOUL);
            } else if (isSpiritPresentOldValue) {
                value += (((int) spiritType + 1) << LevelSyntax.BITSHIFT_SPIRIT);
                value += spiritLevel;
            }
        }

        levelBuilderBackend.UpdateValueAtTileUnit(value, tileCoordinates);
    }
}
