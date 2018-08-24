using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulNigimitama : Soul {
    public override void Start() {
        base.Start();
        soulType = CustomEnums.SoulType.NIGIMITAMA;
    }
}
