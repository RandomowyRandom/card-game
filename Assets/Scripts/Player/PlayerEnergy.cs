using Mirror;
using Scriptables.Player;
using UnityEngine;

namespace Player
{
    public class PlayerEnergy : NetworkBehaviour
    {
        [SerializeField]
        private PlayerEnergyStats _playerBaseEnergyStats;
        
        [SyncVar]
        private int _currentEnergy;
        
        public int CurrentEnergy => _currentEnergy;

        public override void OnStartClient()
        {
            CmdSetEnergy(_playerBaseEnergyStats.StartingEnergy);
        }
        
        public void AddEnergy(int amount)
        {
            CmdSetEnergy(_currentEnergy + amount);
        }
        
        public void RemoveEnergy(int amount)
        {
            CmdSetEnergy(_currentEnergy - amount);
        }

        #region Networking

        [Command]
        private void CmdSetEnergy(int energy)
        {
            _currentEnergy = energy;
        }

        #endregion
    }
}