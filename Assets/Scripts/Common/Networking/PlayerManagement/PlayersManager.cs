using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace Common.Networking.PlayerManagement
{
    public class PlayersManager: NetworkBehaviour, IPlayersManager
    {
        private readonly SyncDictionary<int, NetworkIdentity> _players = new();

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayersManager>();
            _players.Clear();
        }
        
        private void Awake()
        {
            if(ServiceLocator.ServiceLocator.Instance.IsRegistered<IPlayersManager>())
                return;
            
            ServiceLocator.ServiceLocator.Instance.Register<IPlayersManager>(this);
        }
        
        public void RegisterPlayer(NetworkIdentity player, int connectionId)
        {
            if (_players.ContainsKey(connectionId))
                return;
            
            _players.Add(connectionId, player);
        }
        
        public void DeregisterPlayer(int connectionId)
        {
            if (!_players.ContainsKey(connectionId))
                return;
            
            _players.Remove(connectionId);
        }
        
        public NetworkIdentity GetPlayer(int connectionId)
        {
            return !_players.ContainsKey(connectionId) ? null : _players[connectionId];
        }
        
        public List<NetworkIdentity> GetAllPlayers()
        {
            return _players.Values.ToList();
        }
    }
}