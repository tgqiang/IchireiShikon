using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSakimitama : Soul {

    protected override void Awake () {
        base.Awake();
        soulType = SoulType.SAKIMITAMA;
    }

    public override void AttemptMerge () {
        base.AttemptMerge();
    }
}
