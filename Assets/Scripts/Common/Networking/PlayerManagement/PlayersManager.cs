using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using Player;
using UnityEngine;

namespace Common.Networking.PlayerManagement
{
    public class PlayersManager: NetworkBehaviour, IPlayersManager
    {
        public event Action OnPlayersChanged;

        private readonly SyncDictionary<int, NetworkIdentity> _players = new();

        private void OnDestroy()
        {
            _players.Callback -= PlayersChanged;

            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayersManager>();
            _players.Clear();
        }
        
        private void Awake()
        {
            _players.Callback += PlayersChanged;
            
            if(ServiceLocator.ServiceLocator.Instance.IsRegistered<IPlayersManager>())
                return;
            
            ServiceLocator.ServiceLocator.Instance.Register<IPlayersManager>(this);
        }

        public void RefreshPlayers()
        {
            CmdClearPlayers();
            var players = FindObjectsOfType<PlayerHealth>();
            
            foreach (var player in players)
            {
                var networkIdentity = player.GetComponent<NetworkIdentity>();
                CmdAddPlayer(networkIdentity, networkIdentity.connectionToClient.connectionId);
                Debug.Log($"Player {networkIdentity.connectionToClient.connectionId} added to the dict");
            }
        }
        
        public void DeregisterPlayer(NetworkIdentity player)
        {
            CmdRemovePlayer(player.connectionToClient.connectionId);
        }

        public NetworkIdentity GetPlayer(int connectionId)
        {
            return !_players.ContainsKey(connectionId) ? null : _players[connectionId];
        }

        public NetworkIdentity GetLocalPlayer()
        {
            return _players.FirstOrDefault(x => x.Value.isOwned).Value;
        }

        public List<NetworkIdentity> GetAllPlayers(bool includeLocalPlayer = true)
        {
            return includeLocalPlayer ? _players.Values.ToList() : _players.Values.ToList().Where(p => !p.isOwned).ToList();
        }
        
        private void PlayersChanged(SyncDictionary<int, NetworkIdentity>.Operation op, int key, NetworkIdentity value)
        {
            OnPlayersChanged?.Invoke();
        }

        #region Networking

        [Command(requiresAuthority = false)]
        private void CmdAddPlayer(NetworkIdentity player, int connectionId)
        {
            _players.Add(connectionId, player);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdRemovePlayer(int connectionId)
        {
            _players.Remove(connectionId);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdClearPlayers()
        {
            _players.Clear();
        }

        #endregion

        #region QC

        [QFSW.QC.Command("log-all-players")] [UsedImplicitly]
        private void LogPlayers()
        {
            foreach (var player in _players)
            {
                Debug.Log($"Player: {player.Value.name} - ConnectionId: {player.Key}");
            }
        }

        #endregion
    }
}