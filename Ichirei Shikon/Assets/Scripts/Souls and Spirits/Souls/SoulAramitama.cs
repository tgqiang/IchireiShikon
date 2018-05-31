using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAramitama : Soul {

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        soulType = SoulType.ARAMITAMA;
	}

    public override void AttemptMerge () {
        base.AttemptMerge();
    }
}
