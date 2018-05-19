using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSakimitama : Soul {

    protected override void Start () {
        base.Start();
        soulType = SoulType.SAKIMITAMA;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }
}
