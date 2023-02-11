using System;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RoundButton: MonoBehaviour, IRoundButton
    {
        [SerializeField]
        private Button _button;
        
        private IRoundManager _roundManager;
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IRoundButton>(this);
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToEvents;
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IRoundButton>();
            
            _button.onClick.RemoveListener(_roundManager.NextRound);
            
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }
        
        public void BlockButton(bool block)
        {
            _button.interactable = !block;
        }
        
        private void SubscribeToEvents(Type type)
        {
            if(type != typeof(IRoundManager))
                return;
            
            _roundManager = ServiceLocator.ServiceLocator.Instance.Get<IRoundManager>();
            
            _button.onClick.AddListener(_roundManager.NextRound);
        }
        
    }
}