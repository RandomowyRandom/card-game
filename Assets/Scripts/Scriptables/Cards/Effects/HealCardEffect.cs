using System;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class HealCardEffect: ICardEffect
    {
        [SerializeField]
        private int _healAmount;

        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();
            
            foreach (var target in targets)
            {
                var playerHealth = target.GetComponent<IPlayerHealth>();
                playerHealth.Heal(_healAmount);
            }
            
            Debug.Log($"Healed self for {_healAmount}!");
        }
    }
}