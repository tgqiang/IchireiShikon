using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulNigimitama : Soul {

    protected override void Start () {
        base.Start();
        soulType = SoulType.NIGIMITAMA;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }
}
