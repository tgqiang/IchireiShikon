using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAramitama : Soul {
    public override void Start() {
        base.Start();
        soulType = CustomEnums.SoulType.ARAMITAMA;
    }
}
