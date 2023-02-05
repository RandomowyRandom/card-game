using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ServiceLocator
{
    public class ServiceLocatorDebugger: SerializedMonoBehaviour
    {
        [OdinSerialize] [ReadOnly]
        private List<string> _registeredServices = new();

        private void Awake()
        {
            ServiceLocator.Instance.OnServiceRegistered += AddService;
            ServiceLocator.Instance.OnServiceDeregistered += RemoveService;
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.OnServiceRegistered -= AddService;
            ServiceLocator.Instance.OnServiceDeregistered -= RemoveService;
        }

        private void AddService(Type type)
        {
            _registeredServices.Add(type.Name);
        }
        
        private void RemoveService(Type type)
        {
            _registeredServices.Remove(type.Name);
        }
    }
}