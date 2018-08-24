using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfLove : Spirit {

    public override void Start() {
        base.Start();
        spiritType = CustomEnums.SpiritType.LOVE;
    }

    public override void TriggerEffect() {

    }
}
