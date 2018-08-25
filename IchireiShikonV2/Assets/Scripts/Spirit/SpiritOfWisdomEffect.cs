using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfWisdomEffect : SpiritEffect {

    public override void PerformEffect(Tile target) {
        target.PurifyWithBarrier();
    }
}
