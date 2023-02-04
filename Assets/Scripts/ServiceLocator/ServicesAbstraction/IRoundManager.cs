using System;
using Player;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IRoundManager: IService
    {
        public event Action<PlayerData> OnRoundStarted;
        public event Action<PlayerData> OnRoundEnded;
        
        public void StartRound(PlayerData playerData);
        
        public void EndRound(PlayerData playerData);
    }
}