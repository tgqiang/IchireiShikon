using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritLove : Spirit {

    protected override void Start () {
        base.Start();
        spiritType = SpiritType.LOVE;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

}
