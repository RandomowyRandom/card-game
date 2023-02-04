using TMPro;
using UnityEngine;

namespace Player.Username
{
    public class MockUsernameProvider: MonoBehaviour, IUsernameProvider
    {
        [SerializeField]
        private TMP_InputField _inputField;

        private void Awake()
        {
            if(ServiceLocator.ServiceLocator.Instance.IsRegistered<IUsernameProvider>())
                return;
            
            ServiceLocator.ServiceLocator.Instance.Register<IUsernameProvider>(this);
        }

        public string GetUsername()
        {
            return _inputField.text;
        }
    }
}