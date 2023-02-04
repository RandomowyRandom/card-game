using System;
using System.Collections.Generic;
using System.Linq;
using Common.Networking.PlayerManagement;
using JetBrains.Annotations;
using Mirror;
using Player;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

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

        [Server]
        public void StartRound(PlayerData playerData)
        {
            _currentPlayer = playerData;
            
            RpcStartRound(playerData);
        }

        [Server]
        public void EndRound(PlayerData playerData)
        {
            _roundQueue.Enqueue(playerData);
            
            RpcEndRound(playerData);
        }

        [ContextMenu("Start Queue")]
        private void StartQueue()
        {
            var players = PlayersManager.GetAllPlayers();

            foreach (var playerData in players.Select(player => player.GetComponent<PlayerData>()))
            {
                _roundQueue.Enqueue(playerData);
            }
            
            NextRound();
        }
        
        [ContextMenu("Next Round")]
        private void NextRound()
        {
            StartRound(_roundQueue.Dequeue());
            EndRound(_currentPlayer);
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
        
        #endregion
    }
}