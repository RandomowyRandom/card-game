using Mirror;
using Scriptables.Player;
using UnityEngine;

namespace Player
{
    public class PlayerWallet : NetworkBehaviour
    {
        [SerializeField]
        private PlayerWalletStats _playerBaseWalletStats;
        
        [SyncVar]
        private int _currentMoney;

        public int MoneyAmount => _currentMoney;
        
        public void AddMoney(int amount)
        {
            CmdSetMoney(_currentMoney + amount);
        }
        
        public void SubtractMoney(int amount)
        {
            CmdSetMoney(_currentMoney - amount);
        }
        
        public bool CanAfford(int amount)
        {
            return _currentMoney >= amount;
        }
        
        public void SetMoney(int amount)
        {
            CmdSetMoney(amount);
        }
        
        public override void OnStartClient()
        {
            CmdSetMoney(_playerBaseWalletStats.StartingMoney);
        }
        
        #region Networking

        [Command]
        private void CmdSetMoney(int money)
        {
            _currentMoney = money;
        }
        
        #endregion
    }
}