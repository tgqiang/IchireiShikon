using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfWisdom : Spirit {

    const int AREAS_OF_EFFECT_MAX = 13;

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
        switch (spiritLevel) {
            case 4:
            case 3:
                for (int i = 5; i < 5 + (spiritLevel - 2) * 4; i++) {
                    areaOfEffect[i].enabled = true;
                }
                goto case 2;
            case 2:
            case 1:
                for (int i = 0; i <= spiritLevel * 2; i++) {
                    areaOfEffect[i].enabled = true;
                }
                break;
            default:
                break;
        }
    }

    public override void HideAreaOfEffect() {
        switch (spiritLevel) {
            case 4:
            case 3:
                for (int i = 5; i < 5 + (spiritLevel - 2) * 4; i++) {
                    areaOfEffect[i].enabled = false;
                }
                goto case 2;
            case 2:
            case 1:
                for (int i = 0; i <= spiritLevel * 2; i++) {
                    areaOfEffect[i].enabled = false;
                }
                break;
            default:
                break;
        }
    }

    public override void TriggerEffect() {
        Tile[][] tileMap = Tile.gameLevel.levelData.tileMap;
        Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;
        Vector2Int coords = CurrentLocation.tileCoords;

        switch (spiritLevel) {
            case 4:
                if (coords.x - 2 >= 0 && tileMap[coords.x - 2][coords.y] != null)
                    tileMap[coords.x - 2][coords.y].PurifyWithBarrier();
                if (coords.x + 2 < mapBounds.x && tileMap[coords.x + 2][coords.y] != null)
                    tileMap[coords.x + 2][coords.y].PurifyWithBarrier();
                if (coords.y - 2 >= 0 && tileMap[coords.x][coords.y - 2] != null)
                    tileMap[coords.x][coords.y - 2].PurifyWithBarrier();
                if (coords.y + 2 < mapBounds.y && tileMap[coords.x][coords.y + 2] != null)
                    tileMap[coords.x][coords.y + 2].PurifyWithBarrier();
                goto case 3;
            case 3:
                if (coords.x - 1 >= 0 && coords.y - 1 >= 0 && tileMap[coords.x - 1][coords.y - 1] != null)
                    tileMap[coords.x - 1][coords.y - 1].PurifyWithBarrier();
                if (coords.x - 1 >= 0 && coords.y + 1 < mapBounds.y && tileMap[coords.x - 1][coords.y + 1] != null)
                    tileMap[coords.x - 1][coords.y + 1].PurifyWithBarrier();
                if (coords.x + 1 < mapBounds.x && coords.y - 1 >= 0 && tileMap[coords.x + 1][coords.y - 1] != null)
                    tileMap[coords.x + 1][coords.y - 1].PurifyWithBarrier();
                if (coords.x + 1 < mapBounds.x && coords.y + 1 < mapBounds.y && tileMap[coords.x + 1][coords.y + 1] != null)
                    tileMap[coords.x + 1][coords.y + 1].PurifyWithBarrier();
                goto case 2;
            case 2:
                if (coords.x - 1 >= 0 && tileMap[coords.x - 1][coords.y] != null) tileMap[coords.x - 1][coords.y].PurifyWithBarrier();
                if (coords.x + 1 < mapBounds.x && tileMap[coords.x + 1][coords.y] != null) tileMap[coords.x + 1][coords.y].PurifyWithBarrier();
                goto case 1;
            case 1:
                if (coords.y - 1 >= 0 && tileMap[coords.x][coords.y - 1] != null) tileMap[coords.x][coords.y - 1].PurifyWithBarrier();
                if (coords.y + 1 < mapBounds.y && tileMap[coords.x][coords.y + 1] != null) tileMap[coords.x][coords.y + 1].PurifyWithBarrier();
                tileMap[coords.x][coords.y].PurifyWithBarrier();
                DegradeAfterTrigger();
                break;
            default:
                break;
        }
    }
}
