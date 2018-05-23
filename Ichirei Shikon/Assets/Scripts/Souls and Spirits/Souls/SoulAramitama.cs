using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAramitama : Soul {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        soulType = SoulType.ARAMITAMA;
	}

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }
}
