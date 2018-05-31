using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulKushimitama : Soul {

    protected override void Awake () {
        base.Awake();
        soulType = SoulType.KUSHIMITAMA;
    }

    public override void AttemptMerge () {
        base.AttemptMerge();
    }
}
