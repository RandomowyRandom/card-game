namespace Buffs
{
    public class PlayerBuff
    {
        public PlayerBuff(Buff buff)
        {
            Buff = buff;
            RemainingRounds = buff.RoundDuration;
        }

        public void DecreaseRemainingRounds()
        {
            RemainingRounds--;
        }
        
        public Buff Buff { get; }
        public int RemainingRounds { get; private set; }
    }
}