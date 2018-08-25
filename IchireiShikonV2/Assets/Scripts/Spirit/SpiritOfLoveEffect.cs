using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfLoveEffect : SpiritEffect {

    public override void PerformEffect(Tile target) {
        if (!target.IsVacant()) {
            target.objectOnTile.Purify();
        }

        target.Purify();
    }
}
