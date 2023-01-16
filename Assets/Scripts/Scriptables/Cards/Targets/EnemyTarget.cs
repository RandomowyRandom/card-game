using System;
using Common.Networking.PlayerManagement;
using Mirror;
using Scriptables.Cards.Abstractions;

namespace Scriptables.Cards.Targets
{
    [Serializable]
    public class EnemyTarget: ITargetProvider
    {
        private IPlayersManager _playersManager;
        public NetworkIdentity[] GetTargets()
        {
            _playersManager ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayersManager>();

            return new []{_playersManager.GetLocalPlayer()} ; //TODO: get enemy instead of local player
        }
    }
}