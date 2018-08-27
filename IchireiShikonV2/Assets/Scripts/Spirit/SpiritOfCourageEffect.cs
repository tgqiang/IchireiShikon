using UnityEngine;

public class SpiritOfCourageEffect : SpiritEffect {

    SpriteRenderer effectMarker;
    Tile effectCenter;

    void Awake() {
        effectMarker = GetComponent<SpriteRenderer>();
        if (effectMarker == null) {
            throw new System.Exception("Child SpriteRenderer component missing from Mergeable object.");
        }
    }

    public void ShowAreaOfEffect(int spiritLevel) {
        effectMarker.size = Vector2.one * (2 * spiritLevel + 1);
        effectMarker.enabled = true;
    }

    public void HideAreaOfEffect() {
        effectMarker.enabled = false;
    }

    public override void ExertEffect(Vector2Int centerCoords, Vector2Int mapBounds, Tile[][] tileMap) {
        int spiritLevel = GetComponentInParent<Spirit>().SpiritLevel;
        effectCenter = tileMap[centerCoords.x][centerCoords.y];

        int rowMin = Mathf.Max(0, centerCoords.x - spiritLevel);
        int rowMax = Mathf.Min(mapBounds.x - 1, centerCoords.x + spiritLevel);
        int colMin = Mathf.Max(0, centerCoords.y - spiritLevel);
        int colMax = Mathf.Min(mapBounds.y - 1, centerCoords.y + spiritLevel);

        for (int row = rowMin; row <= rowMax; row++) {
            for (int col = colMin; col <= colMax; col++) {
                Tile target = tileMap[row][col];
                if (target != null) {
                    PerformEffect(target);
                }
            }
        }
    }

    public override void PerformEffect(Tile target) {
        if (GetComponentInParent<Spirit>().IsTainted) {
            target.Taint();
        } else {
            target.Purify();
            if (target != effectCenter && !target.IsVacant()) {
                Tile.RemoveFromTileAndDestroy(target.objectOnTile, target);
            }
        }
    }
}
