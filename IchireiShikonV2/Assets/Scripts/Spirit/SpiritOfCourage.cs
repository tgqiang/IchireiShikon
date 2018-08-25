using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfCourage : Spirit {

    [SerializeField]
    SpriteRenderer areaOfEffect;

    public override void Start() {
        base.Start();
        if (areaOfEffect == null) {
            throw new System.Exception("Child SpriteRenderer component missing from Mergeable object.");
        }
        spiritType = CustomEnums.SpiritType.COURAGE;
    }

    public override void ShowAreaOfEffect() {
        areaOfEffect.size = Vector2.one * (2 * spiritLevel + 1);
        areaOfEffect.enabled = true;
    }

    public override void HideAreaOfEffect() {
        areaOfEffect.enabled = false;
    }

    public override void TriggerEffect() {
        Tile[][] tileMap = Tile.gameLevel.levelData.tileMap;
        Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;

        int rowMin = Mathf.Max(0, CurrentLocation.tileCoords.x - spiritLevel);
        int rowMax = Mathf.Min(mapBounds.x - 1, CurrentLocation.tileCoords.x + spiritLevel);
        int colMin = Mathf.Max(0, CurrentLocation.tileCoords.y - spiritLevel);
        int colMax = Mathf.Min(mapBounds.y - 1, CurrentLocation.tileCoords.y + spiritLevel);

        for (int row = rowMin; row <= rowMax; row++) {
            for (int col = colMin; col <= colMax; col++) {
                Tile target = tileMap[row][col];
                if (target != null) {
                    target.Purify();
                    if (CurrentLocation.tileCoords == new Vector2Int(row, col)) {
                        DegradeAfterTrigger();
                        continue;
                    }
                    if (!target.IsVacant()) {
                        FindObjectOfType<ObjectSpawner>().RemoveObjectFromGame(target.objectOnTile.gameObject);
                        target.objectOnTile = null;
                    }
                }
            }
        }
    }
}
