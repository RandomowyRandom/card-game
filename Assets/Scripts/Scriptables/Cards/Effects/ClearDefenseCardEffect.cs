using System;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class ClearDefenseCardEffect : ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();
            
            foreach (var target in targets)
            {
                var playerHealth = target.GetComponent<IPlayerHealth>();
                playerHealth.SetArmor(0);
            }
            
            Debug.Log("Cleared enemy Defense");
        }
    }
}