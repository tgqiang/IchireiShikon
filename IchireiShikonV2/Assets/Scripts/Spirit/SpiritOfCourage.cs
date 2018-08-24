using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfCourage : Spirit {

    public override void Start() {
        base.Start();
        spiritType = CustomEnums.SpiritType.COURAGE;
    }

    public override void TriggerEffect() {

    }
}
