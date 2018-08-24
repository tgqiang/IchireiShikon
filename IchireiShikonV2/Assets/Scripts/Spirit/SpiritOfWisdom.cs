using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfWisdom : Spirit {

    public override void Start() {
        base.Start();
        spiritType = CustomEnums.SpiritType.WISDOM;
    }

    public override void TriggerEffect() {
        
    }
}
