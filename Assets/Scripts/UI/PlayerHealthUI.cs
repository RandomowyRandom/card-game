using System;
using Mirror;
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
        private TMP_Text _armorText;
        
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
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToEvents;
        }
        
        private void OnDestroy()
        {
            PlayerHealth.OnHealthChanged -= UpdateHealthUI;
            PlayerHealth.OnArmorChanged -= UpdateArmorUI;
            
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }
        
        private void UpdateHealthUI(int health)
        {
            _healthText.text = $"{health.ToString()} / {PlayerHealth.MaxHealth}";
        }
        private void UpdateArmorUI(int armor)
        {
            _armorText.text = armor.ToString();
        }
        
        private void SubscribeToEvents(Type type)
        {
            if (type != typeof(IPlayerHealth)) 
                return;
            
            PlayerHealth.OnHealthChanged += UpdateHealthUI;
            PlayerHealth.OnArmorChanged += UpdateArmorUI;
            
            UpdateHealthUI(PlayerHealth.CurrentHealth);
            UpdateArmorUI(0);
        }

    }
}