using Common.Networking.PlayerManagement;
using Mirror;

namespace Common.Networking
{
    public class GameNetworkManager: NetworkManager
    {
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
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
            _playersManager.RefreshPlayers();
        }
    }
}