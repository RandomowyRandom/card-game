using System.Collections.Generic;
using System.Linq;
using Common.Networking.PlayerManagement;
using Mirror;
using Player;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Common.Managers
{
    public class VictoryManager: NetworkBehaviour, IVictoryManager
    {
        private IPlayersManager _playersManager;
        private IPlayersManager PlayersManager => _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
        
        private List<IPlayerHealth> _playersHealth = new();
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IVictoryManager>(this);
        }

        private void Start()
        {
            PlayerHealth.OnDeath += TryFinishGame;
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IVictoryManager>();
            
            PlayerHealth.OnDeath -= TryFinishGame;
        }

        private void TryFinishGame(IPlayerData playerData)
        {
            var players = PlayersManager.GetAllPlayers().
                Select(p => p.GetComponent<PlayerHealth>()).
                Where(p => p.IsAlive).
                ToList();
            
            if (players.Count == 1)
                Debug.Log($"Player {players[0].GetComponent<IPlayerData>().Username} won the game!");
        }
    }
}