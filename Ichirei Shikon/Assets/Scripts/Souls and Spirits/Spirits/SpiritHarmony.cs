using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHarmony : Spirit {

    protected override void Start () {
        base.Start();
        spiritType = SpiritType.HARMONY;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

}
