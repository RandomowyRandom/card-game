using System;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DamageCardEffect : ICardEffect
    {
        [SerializeField]
        private int _damage;

        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();
            
            foreach (var target in targets)
            {
                var playerHealth = target.GetComponent<IPlayerHealth>();
                playerHealth.TakeDamage(_damage);
            }
            
            Debug.Log($"Deal {_damage} damage!");
        }
    }
}