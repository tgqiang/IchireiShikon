using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritCourage : Spirit {

    protected override void Start () {
        base.Start();
        spiritType = SpiritType.COURAGE;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

}
