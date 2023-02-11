using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

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

        #region QC

        [QFSW.QC.Command("sl-log-services")] [UsedImplicitly]
        private void CommandLogServices()
        {
            foreach (var service in _registeredServices)
                Debug.Log(service);
        }


        #endregion
    }
}