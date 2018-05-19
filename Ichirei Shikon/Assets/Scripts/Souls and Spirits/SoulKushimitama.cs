using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulKushimitama : Soul {

    protected override void Start () {
        base.Start();
        soulType = SoulType.KUSHIMITAMA;
    }

    protected override void AttemptMerge () {
        base.AttemptMerge();
    }
}
