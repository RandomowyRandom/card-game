using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class RemotePlayerStats: MonoBehaviour
    {
        [SerializeField]
        private NetworkIdentity _parentNetworkIdentity;
        
        [SerializeField]
        private TMP_Text _connectionIdText;
        
        private Camera _mainCamera;

        private void Start()
        {
            if (_parentNetworkIdentity.isOwned)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _mainCamera = Camera.main;

            _connectionIdText.text = _parentNetworkIdentity.isServer ?
                _parentNetworkIdentity.connectionToClient.connectionId.ToString()
                : string.Empty;
        }

        private void Update()
        { 
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        }
    }
}