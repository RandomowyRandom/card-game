using System;
using Mirror;
using Scriptables.Player;
using ServiceLocator.ServicesAbstraction;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerHealthUI: NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text _healthText;
        
        [SerializeField]
        private PlayerHealthStats _playerHealthStats;

        private IPlayerHealth _playerHealth;

        private IPlayerHealth PlayerHealth
        {
            get
            {
                _playerHealth ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHealth>();
                return _playerHealth;
            }
        }

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToHealthEvent;
        }
        
        private void OnDestroy()
        {
            PlayerHealth.OnHealthChanged -= UpdateUI;
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToHealthEvent;
        }
        
        private void UpdateUI(int health)
        {
            _healthText.text = $"{health.ToString()} / {_playerHealthStats.MaxHealth}";
        }
        
        private void SubscribeToHealthEvent(Type type)
        {
            if (type != typeof(IPlayerHealth)) 
                return;
            
            PlayerHealth.OnHealthChanged += UpdateUI;
            UpdateUI(PlayerHealth.CurrentHealth);
        }
    }
}