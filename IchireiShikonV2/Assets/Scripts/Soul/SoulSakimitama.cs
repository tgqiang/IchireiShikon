using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSakimitama : Soul {
    public override void Start() {
        base.Start();
        soulType = CustomEnums.SoulType.SAKIMITAMA;
    }
}
