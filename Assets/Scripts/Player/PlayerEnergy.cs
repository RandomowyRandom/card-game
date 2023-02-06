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
    public class PlayerEnergy : NetworkBehaviour, IPlayerEnergy
    {
        [SerializeField]
        private PlayerEnergyStats _playerBaseEnergyStats;
        
        [SyncVar(hook=nameof(OnEnergyChangedHook))]
        private int _currentEnergy;
        public event Action<int> OnEnergyChanged;
        public int CurrentEnergy => _currentEnergy;
        public int MaxEnergy => _playerBaseEnergyStats.MaxEnergy;

        private void OnDestroy()
        {
            if (!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerEnergy>();
        }
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerEnergy>(this);
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

        public void SetEnergy(int amount)
        {
            CmdSetEnergy(amount);
        }

        public void ResetEnergy()
        {
            SetEnergy(_playerBaseEnergyStats.MaxEnergy);
        }
        
        private void OnEnergyChangedHook(int oldEnergy, int newEnergy)
        {
            OnEnergyChanged?.Invoke(newEnergy);
        }
        
        #region Networking

        [Mirror.Command]
        private void CmdSetEnergy(int energy)
        {
            _currentEnergy = energy;
        }

        #endregion

        #region QC

        [QFSW.QC.Command("set-energy")] [UsedImplicitly]
        private IEnumerator<ICommandAction> CommandSetEnergy(int energy)
        {
            PlayerEnergy target = default;
            
            var targets = InvocationTargetFactory.FindTargets<PlayerEnergy>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerEnergy>(targets, t => target = t);
            
            target.SetEnergy(energy);
            Debug.Log($"Changed {target.name} to {energy} energy");
        }
        
        [QFSW.QC.Command("add-energy")] [UsedImplicitly]
        private IEnumerator<ICommandAction> CommandAddEnergy(int energy)
        {
            PlayerEnergy target = default;
            
            var targets = InvocationTargetFactory.FindTargets<PlayerEnergy>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerEnergy>(targets, t => target = t);
            
            target.AddEnergy(energy);
            Debug.Log($"Added {energy} to {target.name}");
        }
        
        [QFSW.QC.Command("remove-energy")] [UsedImplicitly]
        private IEnumerator<ICommandAction> CommandRemoveEnergy(int energy)
        {
            PlayerEnergy target = default;
            
            var targets = InvocationTargetFactory.FindTargets<PlayerEnergy>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerEnergy>(targets, t => target = t);
            
            target.RemoveEnergy(energy);
            Debug.Log($"Removed {energy} from {target.name}");
        }

        #endregion
    }
}