using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ServiceLocator
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        private static Dictionary<Type, IService> _registeredServices;
        
        // prevents instantiation
        private ServiceLocator() { }

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                
                _instance = new ServiceLocator();
                _registeredServices = new();

                return _instance;
            }
        }
        
        public void Register<T>(T service ) where T : IService
        {
            if(_registeredServices.ContainsKey(typeof(T)))
                Debug.LogWarning($"Service of type {service.GetServiceType()} is already registered");
            
            _registeredServices.Add(typeof(T), service);
        }
        
        public void DynamicRegister(Type typeInfo, IService objectInstance)
        {
            if(_registeredServices.ContainsKey(typeInfo))
                Debug.LogWarning($"Service of type {typeInfo} is already registered");
            
            _registeredServices.Add(typeInfo, objectInstance);
        }

        public void ForceRegister<T>(T service) where T : IService
        {
            _registeredServices[typeof(T)] = service;
            Debug.LogWarning($"ServiceLocator: ForceRegister: {typeof(T)}");
        }
        
        public void Deregister<T>() where T : IService
        {
            _registeredServices.Remove(typeof(T));
        }
        
        public T Get<T>() where T : IService
        {
            _registeredServices.TryGetValue(typeof(T), out var result);

            return (T)result;
        }
        
        public void DeregisterAll()
        {
            _registeredServices.Clear();
        }
    }
}