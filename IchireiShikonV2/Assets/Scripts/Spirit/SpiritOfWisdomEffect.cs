public class SpiritOfWisdomEffect : SpiritEffect {

    public override void PerformEffect(Tile target) {
        target.PurifyWithBarrier();
    }
}
