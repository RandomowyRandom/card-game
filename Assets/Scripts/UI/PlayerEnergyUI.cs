using System;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerEnergyUI: NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text _energyText;

        private IPlayerEnergy _playerEnergy;
        
        private IPlayerEnergy PlayerEnergy
        {
            get
            {
                _playerEnergy ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerEnergy>();
                return _playerEnergy;
            }
        }
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToEvents;
        }

        private void OnDestroy()
        {
            PlayerEnergy.OnEnergyChanged -= UpdateEnergyUI;
            
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }

        private void UpdateEnergyUI(int energy)
        {
            _energyText.text = $"{energy.ToString()}/{PlayerEnergy.MaxEnergy}";
        }
        
        private void SubscribeToEvents(Type type)
        {
            if (type != typeof(IPlayerEnergy)) 
                return;
            
            PlayerEnergy.OnEnergyChanged += UpdateEnergyUI;
        }
    }
}