using UnityEngine;

public class SpiritOfFriendship : Spirit {

    const int AREAS_OF_EFFECT = 8;
    const int AREAS_OF_EFFECT_PER_LEVEL = 2;

    [SerializeField]
    SpiritOfFriendshipEffect[] areaOfEffect;

    public override void Start() {
        base.Start();
        if (areaOfEffect.Length != AREAS_OF_EFFECT) {
            throw new System.Exception("Array of areas of effect is of incorrect length.");
        }
        spiritType = CustomEnums.SpiritType.FRIENDSHIP;
    }

    public override void ShowAreaOfEffect() {
        for (int i = 0; i < spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].RevealEffectMarker();
        }
    }

    public override void HideAreaOfEffect() {
        for (int i = 0; i < spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].HideEffectMarker();
        }
    }

    public override void TriggerEffect() {
        Tile[][] tileMap = Tile.gameLevel.levelData.tileMap;
        Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;
        Vector2Int currentCoords = CurrentLocation.tileCoords;

        for (int i = 0; i < spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].ExertEffect(currentCoords, mapBounds, tileMap);
        }

        DegradeAfterTrigger();
    }
}
