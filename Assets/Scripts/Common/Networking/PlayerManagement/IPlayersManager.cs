using System.Collections.Generic;
using Mirror;
using ServiceLocator;

namespace Common.Networking.PlayerManagement
{
    public interface IPlayersManager: IService
    {
        public void RegisterPlayer(NetworkIdentity player, int connectionId);
        public void DeregisterPlayer(int connectionId);
        public NetworkIdentity GetPlayer(int connectionId);
        public NetworkIdentity GetLocalPlayer();
        public List<NetworkIdentity> GetAllPlayers(bool includeLocalPlayer = true);
    }
}