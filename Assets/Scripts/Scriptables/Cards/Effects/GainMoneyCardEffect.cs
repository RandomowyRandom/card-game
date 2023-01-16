using System;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class GainMoneyCardEffect : ICardEffect
    {
        [SerializeField]
        private int _money;

        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();
            
            foreach (var target in targets)
            {
                var playerHealth = target.GetComponent<IPlayerWallet>();
                playerHealth.AddMoney(_money);
            }
            
            Debug.Log($"Enemy gained {_money} money");
        }
    }
}