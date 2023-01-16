using System;
using Scriptables.Cards.Abstractions;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class CardUpgradeCardEffect : ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            Debug.Log("Upgraded card!");
        }
    }
}