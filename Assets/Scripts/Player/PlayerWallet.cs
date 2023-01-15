using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using QFSW.QC;
using QFSW.QC.Actions;
using Scriptables.Player;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerWallet : NetworkBehaviour, IPlayerWallet
    {
        [SerializeField]
        private PlayerWalletStats _playerBaseWalletStats;
        
        [SyncVar(hook=nameof(OnMoneyChangedHook))]
        private int _currentMoney;

        public event Action<int> OnMoneyChanged;
        
        public int MoneyAmount => _currentMoney;
        
        private void OnDestroy()
        {
            if (!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerHealth>();
        }
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerWallet>(this);
        }
        
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

        private void OnMoneyChangedHook(int oldMoney, int newMoney)
        {
            OnMoneyChanged?.Invoke(newMoney);
        }

        #region Networking

        [Mirror.Command]
        private void CmdSetMoney(int money)
        {
            _currentMoney = money;
        }
        
        #endregion

        #region QC

        [QFSW.QC.Command("set-money")] [UsedImplicitly]
        private IEnumerator<ICommandAction> SetMoneyCommand(int money)
        {
            PlayerWallet target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerWallet>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerWallet>(targets, t => target = t);

            target.SetMoney(money);
            Debug.Log($"Changed {target.name} to {money} money");
        }
        
        [QFSW.QC.Command("add-money")] [UsedImplicitly]
        private IEnumerator<ICommandAction> AddMoneyCommand(int money)
        {
            PlayerWallet target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerWallet>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerWallet>(targets, t => target = t);

            target.AddMoney(money);
            Debug.Log($"Added {money} money to {target.name}");
        }
        
        [QFSW.QC.Command("remove-money")] [UsedImplicitly]
        private IEnumerator<ICommandAction> RemoveMoneyCommand(int money)
        {
            PlayerWallet target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerWallet>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerWallet>(targets, t => target = t);

            target.SubtractMoney(money);
            Debug.Log($"Removed {money} money from {target.name}");
        }


        #endregion
    }
}