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
            
            _playersManager.RegisterPlayer(player.GetComponent<NetworkIdentity>(), conn.connectionId);
        }
    }
}