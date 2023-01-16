using System;
using Common.Networking.PlayerManagement;
using Mirror;
using UnityEngine;

namespace Common.Networking
{
    public class GameNetworkManager: NetworkManager
    {
        [SerializeField]
        private Transform _playersParent;

        private IPlayersManager _playersManager;

        public override void Start()
        {
            base.Start();

            _playersManager = ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var startPos = GetStartPosition();
            var player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation, _playersParent)
                : Instantiate(playerPrefab, _playersParent);

            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            if (_playersManager == null)
                ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += RegisterPlayer;
            else
                _playersManager.RefreshPlayers();

            void RegisterPlayer(Type type)
            {
                if (type != typeof(IPlayersManager)) 
                    return;

                _playersManager = ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
                _playersManager.RefreshPlayers();
                ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= RegisterPlayer;
            }
        }
    }
}