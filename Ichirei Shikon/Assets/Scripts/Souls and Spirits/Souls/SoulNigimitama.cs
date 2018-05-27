using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulNigimitama : Soul {

    protected override void Awake () {
        base.Awake();
        soulType = SoulType.NIGIMITAMA;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }
}
