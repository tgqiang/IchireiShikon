using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfWisdom : Spirit {

    const int AREAS_OF_EFFECT_MAX = 17;
    const int AREAS_OF_EFFECT_PER_LEVEL = 4;

    [SerializeField]
    SpriteRenderer[] areaOfEffect;

    public override void Start() {
        base.Start();
        if (areaOfEffect.Length != AREAS_OF_EFFECT_MAX) {
            throw new System.Exception("Array of areas of effect is of incorrect length.");
        }
        spiritType = CustomEnums.SpiritType.WISDOM;
    }

    public override void ShowAreaOfEffect() {
        for (int i = 0; i <= spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].enabled = true;
        }
    }

    public override void HideAreaOfEffect() {
        for (int i = 0; i <= spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].enabled = false;
        }
    }

    public override void TriggerEffect() {
        Tile[][] tileMap = Tile.gameLevel.levelData.tileMap;
        Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;
        Vector2Int currentCoords = CurrentLocation.tileCoords;

        for (int i = 0; i <= spiritLevel * AREAS_OF_EFFECT_PER_LEVEL; i++) {
            areaOfEffect[i].GetComponent<SpiritOfWisdomEffect>().ExertEffect(currentCoords, mapBounds, tileMap);
        }

        DegradeAfterTrigger();
    }
}
