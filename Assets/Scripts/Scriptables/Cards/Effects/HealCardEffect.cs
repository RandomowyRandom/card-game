using System;
using Cysharp.Threading.Tasks;
using Mirror;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class HealCardEffect: ICardEffect
    {
        [SerializeField]
        private int _healAmount;

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
                playerHealth.Heal(_healAmount);
            }
            
            Debug.Log($"Healed self for {_healAmount}!");
        }
    }
}