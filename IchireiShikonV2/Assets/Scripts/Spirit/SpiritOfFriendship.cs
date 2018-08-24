using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfFriendship : Spirit {

    public override void Start() {
        base.Start();
        spiritType = CustomEnums.SpiritType.FRIENDSHIP;
    }

    public override void TriggerEffect() {

    }
}
