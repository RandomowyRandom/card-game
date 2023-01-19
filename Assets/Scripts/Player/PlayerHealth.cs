using System;
using System.Collections.Generic;
using Common.Networking.PlayerManagement;
using JetBrains.Annotations;
using Mirror;
using QFSW.QC;
using QFSW.QC.Actions;
using Scriptables.Player;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour, IPlayerHealth
    {
        [SerializeField]
        private PlayerHealthStats _playerBaseHealthStats;
        
        [SyncVar(hook = nameof(OnHealthChangedHook))]
        private int _currentHealth;
        
        [SyncVar(hook = nameof(OnArmorChangedHook))]
        private int _currentArmor;
        
        public int CurrentHealth => _currentHealth;
        public int CurrentArmor => _currentArmor;

        public int MaxHealth => _playerBaseHealthStats.MaxHealth;
        public event Action<int> OnHealthChanged;
        public event Action<int> OnArmorChanged;
        private void OnDestroy()
        {
            if (!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerHealth>();
        }
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerHealth>(this);
        }
        
        public override void OnStartClient()
        {
            if(!isOwned)
                return;
            
            InitializeStats();
        }
        
        public void Heal(int health)
        {
            CmdSetHealth(_currentHealth + health);
        }
        
        public void AddArmor(int armor)
        {
            CmdSetArmor(_currentArmor + armor);
        }
        
        public void TakeDamage(int damage)
        {
            var damageToTake = damage - _currentArmor;
            
            if (damageToTake < 0)
                damageToTake = 0;
            
            RemoveArmor(damage);
            
            CmdSetHealth(_currentHealth - damageToTake);
        }
        
        public void RemoveArmor(int armor)
        {
            CmdSetArmor(_currentArmor - armor);
        }
        
        public void SetHealth(int health)
        {
            CmdSetHealth(health);
        }
        
        public void SetArmor(int armor)
        {
            CmdSetArmor(armor);
        }

        private void InitializeStats()
        {
            CmdSetHealth(_playerBaseHealthStats.StartingHealth);
            CmdSetArmor(_playerBaseHealthStats.StartingArmor);
        }

        #region Networking
        
        private void OnHealthChangedHook([UsedImplicitly] int oldHealth, int newHealth)
        {
            OnHealthChanged?.Invoke(newHealth);
        }
        
        private void OnArmorChangedHook([UsedImplicitly] int oldArmor, int newArmor)
        {
            OnArmorChanged?.Invoke(newArmor);
        }
        
        [Mirror.Command(requiresAuthority = false)]
        private void CmdSetHealth(int health)
        {
            if(health == 0)
                return;
            
            _currentHealth = health;
            
            if (_currentHealth > _playerBaseHealthStats.MaxHealth)
                _currentHealth = _playerBaseHealthStats.MaxHealth;
        }
        
        [Mirror.Command(requiresAuthority = false)]
        private void CmdSetArmor(int armor)
        {
            if(armor < 0)
                armor = 0;
            
            _currentArmor = armor;
        }
        
        #endregion
        
        #region QC

        [QFSW.QC.Command("set-health")] [UsedImplicitly]
        private IEnumerator<ICommandAction> SetHealthCommand(int health)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.SetHealth(health);
            Debug.Log($"Changed {target.name} to {health} health");
        }
        
        [QFSW.QC.Command("add-health")] [UsedImplicitly]
        private IEnumerator<ICommandAction>AddHealthCommand(int health)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.Heal(health);
            Debug.Log($"Healed {target.name} for {health} health");
        }
        
        [QFSW.QC.Command("remove-health")] [UsedImplicitly]
        private IEnumerator<ICommandAction>RemoveHealthCommand(int health)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.TakeDamage(health);
            Debug.Log($"Damaged {target.name} for {health}");
        }
        
        [QFSW.QC.Command("set-armor")] [UsedImplicitly]
        private IEnumerator<ICommandAction> SetArmorCommand(int armor)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.SetArmor(armor);
            Debug.Log($"Changed {target.name} to {armor} armor");
        }
        
        [QFSW.QC.Command("add-armor")] [UsedImplicitly]
        private IEnumerator<ICommandAction> AddArmorCommand(int armor)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.AddArmor(armor);
            Debug.Log($"Added {armor} armor to {target.name}");
        }
        
        [QFSW.QC.Command("remove-armor")] [UsedImplicitly]
        private IEnumerator<ICommandAction> RemoveArmorCommand(int armor)
        {
            PlayerHealth target = default;

            var targets = InvocationTargetFactory.FindTargets<PlayerHealth>(MonoTargetType.All);

            yield return new Value("Select player");
            yield return new Choice<PlayerHealth>(targets, t => target = t);

            target.RemoveArmor(armor);
            Debug.Log($"Removed {armor} armor from {target.name}");
        }
        
        #endregion
    }
}