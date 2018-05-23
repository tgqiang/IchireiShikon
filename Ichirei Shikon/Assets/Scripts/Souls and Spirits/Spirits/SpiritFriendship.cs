using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFriendship : Spirit {

    protected override void Start () {
        base.Start();
        spiritType = SpiritType.FRIENDSHIP;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }

}
