using UnityEngine;

public class SpiritOfCourage : Spirit {

    [SerializeField]
    SpiritOfCourageEffect areaOfEffect;

    public override void Start() {
        base.Start();
        if (areaOfEffect == null) {
            throw new System.Exception("SpiritOfCourageEffect component missing from Spirit of Courage.");
        }
        spiritType = CustomEnums.SpiritType.COURAGE;
    }

    public override void ShowAreaOfEffect() {
        areaOfEffect.ShowAreaOfEffect(spiritLevel);
    }

    public override void HideAreaOfEffect() {
        areaOfEffect.HideAreaOfEffect();
    }

    public override void TriggerEffect() {
        Tile[][] tileMap = Tile.gameLevel.levelData.tileMap;
        Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;
        Vector2Int currentCoords = CurrentLocation.tileCoords;
        areaOfEffect.ExertEffect(currentCoords, mapBounds, tileMap);
        DegradeAfterTrigger();
    }
}
