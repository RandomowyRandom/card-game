using Mirror;
using Scriptables.Player;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField]
        private PlayerHealthStats _playerBaseHealthStats;
        
        [SyncVar]
        private int _currentHealth;
        
        [SyncVar]
        private int _currentArmor;

        public int CurrentHealth => _currentHealth;
        public int CurrentArmor => _currentArmor;
        
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
        
        public override void OnStartClient()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            CmdSetHealth(_playerBaseHealthStats.StartingHealth);
            CmdSetArmor(_playerBaseHealthStats.StartingArmor);
        }

        #region Networking
        
        [Command]
        private void CmdSetHealth(int health)
        {
            if(health == 0)
                return;
            
            _currentHealth = health;
            if (_currentHealth > _playerBaseHealthStats.MaxHealth)
            {
                _currentHealth = _playerBaseHealthStats.MaxHealth;
            }
        }
        
        [Command]
        private void CmdSetArmor(int armor)
        {
            if(armor < 0)
                armor = 0;
            
            _currentArmor = armor;
        }
        
        #endregion
    }
}