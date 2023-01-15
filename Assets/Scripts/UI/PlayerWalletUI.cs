using System;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerWalletUI: NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text _moneyText;
        
        private IPlayerWallet _playerWallet;
        
        private IPlayerWallet PlayerWallet
        {
            get
            {
                _playerWallet ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerWallet>();
                return _playerWallet;
            }
        }
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToEvents;
        }


        private void OnDestroy()
        {
            PlayerWallet.OnMoneyChanged -= UpdateWalletUI;
            
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }

        private void UpdateWalletUI(int money)
        {
            _moneyText.text = money.ToString();
        }

        private void SubscribeToEvents(Type type)
        {
            if(type != typeof(IPlayerWallet)) 
                return;
            
            PlayerWallet.OnMoneyChanged += UpdateWalletUI;
            
            UpdateWalletUI(PlayerWallet.MoneyAmount);
        }
    }
}