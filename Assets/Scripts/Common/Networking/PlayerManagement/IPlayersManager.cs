using System;
using System.Collections.Generic;
using Mirror;
using ServiceLocator;

namespace Common.Networking.PlayerManagement
{
    public interface IPlayersManager: IService
    {
        public event Action OnPlayersChanged;
        public void RefreshPlayers();
        public void DeregisterPlayer(NetworkIdentity player);
        public NetworkIdentity GetPlayer(int connectionId);
        public NetworkIdentity GetLocalPlayer();
        public List<NetworkIdentity> GetAllPlayers(bool includeLocalPlayer = true);
    }
}