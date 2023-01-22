using System;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using TMPro;
using UnityEngine;

namespace Player
{
    public class RemotePlayerStats: MonoBehaviour
    {
        [SerializeField]
        private NetworkIdentity _parentNetworkIdentity;
        
        [Space]
        
        [SerializeField]
        private TMP_Text _connectionIdText;
        
        [SerializeField]
        private TMP_Text _healthText;
        
        [SerializeField]
        private TMP_Text _armorText;
        
        private Camera _mainCamera;
        
        private IPlayerHealth _playerHealth;
        private IPlayerData _playerData;

        private void Start()
        {
            if (_parentNetworkIdentity.isOwned)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _mainCamera = Camera.main;
            _playerHealth = _parentNetworkIdentity.GetComponent<IPlayerHealth>();
            _playerData = _parentNetworkIdentity.GetComponent<PlayerData>();
            
            _playerHealth.OnHealthChanged += UpdateHealth;
            _playerHealth.OnArmorChanged += UpdateArmor;
            UpdateHealth(_playerHealth.CurrentHealth);
            UpdateArmor(_playerHealth.CurrentArmor);
            
            _playerData.OnUsernameChanged += UpdateUsername;
            UpdateUsername(_playerData.Username);
        }

        private void Update()
        { 
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        }
        
        private void OnDisable()
        {
            if(_parentNetworkIdentity.isOwned)
                return;
            
            _playerHealth.OnHealthChanged -= UpdateHealth;
            _playerHealth.OnArmorChanged -= UpdateArmor;
            _playerData.OnUsernameChanged -= UpdateUsername;
        }

        private void UpdateArmor(int amount)
        {
            _armorText.SetText(amount.ToString());
        }

        private void UpdateHealth(int amount)
        {
            _healthText.SetText(amount.ToString());
        }

        private void UpdateUsername(string username)
        {
            _connectionIdText.SetText(username);
        }
    }
}