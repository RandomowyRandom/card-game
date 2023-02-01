﻿using System;
using Common.Networking.PlayerManagement;
using Mirror;
using Scriptables.Cards.Abstractions;

namespace Scriptables.Cards.Targets
{
    [Serializable]
    public class AllIncludingSelfTarget: ISyncTargetProvider
    {
        private IPlayersManager _playersManager;
        
        public NetworkIdentity[] GetTargets()
        {
            _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();
            
            var players = _playersManager.GetAllPlayers();
            return players.ToArray();
        }
    }
}