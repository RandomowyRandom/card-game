using Buffs;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerBuffHandler: IService
    {
        public void ApplyBuff(Buff buff);
        public void RemoveBuff(PlayerBuff buff);
    }
}