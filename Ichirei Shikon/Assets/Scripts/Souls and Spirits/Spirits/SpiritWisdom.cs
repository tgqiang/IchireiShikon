using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWisdom : Spirit {

    protected override void Start () {
        base.Start();
        spiritType = SpiritType.WISDOM;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

}
