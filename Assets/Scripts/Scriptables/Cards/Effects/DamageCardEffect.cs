using System;
using Cysharp.Threading.Tasks;
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
        public bool IsAsync => TargetProvider is IAsyncTargetProvider;


        public async UniTask OnUse()
        {
            var targets = TargetProvider switch
            {
                ISyncTargetProvider syncTargetProvider => syncTargetProvider.GetTargets(),
                IAsyncTargetProvider asyncTargetProvider => await asyncTargetProvider.GetTargets(),
                _ => null
            };
            
            foreach (var target in targets!)
            {
                var playerHealth = target.GetComponent<IPlayerHealth>();
                playerHealth.TakeDamage(_damage);
            }
            
            Debug.Log($"Deal {_damage} damage!");
        }
    }
}