using System;
using System.Collections.Generic;
using System.Linq;
using Common.Networking.PlayerManagement;
using JetBrains.Annotations;
using Mirror;
using Player;
using ServiceLocator.ServicesAbstraction;

namespace Common.Managers
{
    public class RoundManager: NetworkBehaviour, IRoundManager
    {
        public event Action<PlayerData> OnRoundStarted;
        public event Action<PlayerData> OnRoundEnded;
        
        private Queue<PlayerData> _roundQueue = new();
        
        private PlayerData _currentPlayer;
        
        private IPlayersManager _playersManager;
        
        private IPlayersManager PlayersManager => _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IRoundManager>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IRoundManager>();
        }

        public void NextRound()
        {
            CmdEndRound();
            CmdStartRound();
        }
        private void StartQueue()
        {
            var players = PlayersManager.GetAllPlayers();
            _roundQueue.Clear();
            
            foreach (var playerData in players.Select(player => player.GetComponent<PlayerData>()))
            {
                _roundQueue.Enqueue(playerData);
            }
            
            CmdStartRound();
        }
        
        #region Networking
        
        [ClientRpc(includeOwner = true)]
        private void RpcStartRound(PlayerData playerData)
        {
            OnRoundStarted?.Invoke(playerData);
        }
        
        [ClientRpc(includeOwner = true)]
        private void RpcEndRound(PlayerData playerData)
        {
            OnRoundEnded?.Invoke(playerData);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdStartRound()
        {
            _currentPlayer = _roundQueue.Dequeue();
            
            _roundQueue.Enqueue(_currentPlayer);
            RpcStartRound(_currentPlayer);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdEndRound()
        {
            RpcEndRound(_currentPlayer);
        }

        #endregion

        #region QC

        [QFSW.QC.Command("start-queue", "Starts the queue, can be used only on server")] [UsedImplicitly]
        private void CommandStartQueue()
        {
            if(!isServer)
                return;
            
            StartQueue();
        }

        #endregion
    }
}