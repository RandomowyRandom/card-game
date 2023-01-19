using System;
using System.Linq;
using Common.Networking.PlayerManagement;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerSpotManager: NetworkBehaviour
    {   
        private IPlayersManager _playersManager;

        private IPlayersManager PlayersManager
        {
            get
            {
                _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
                return _playersManager;
            }
        }

        public override void OnStartClient()
        {
            PlayersManager.OnPlayersChanged += UpdatePlayerInstances;
        }

        private void OnDestroy()
        {
            PlayersManager.OnPlayersChanged -= UpdatePlayerInstances;
        }

        private void UpdatePlayerInstances()
        {
            var remotePlayers = PlayersManager.GetAllPlayers(false);

            var spots = GetSpots(remotePlayers.Count);
            
            for (var i = 0; i < remotePlayers.Count; i++)
            {
                var element = remotePlayers[i];

                var spot = GetSpotForPlayer(i, spots);
                element.transform.position = spot.position;
            }
        }

        private Transform GetSpots(int remotePlayerCount)
        {
            var children = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children.FirstOrDefault(child => child.name == remotePlayerCount.ToString());
        }

        private Transform GetSpotForPlayer(int index, Transform spots)
        {
            var children = new Transform[spots.childCount];
            for (var i = 0; i < spots.childCount; i++)
            {
                children[i] = spots.GetChild(i);
            }
            
            return children[index];
        }
    }
}