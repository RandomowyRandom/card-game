using System;
using Scriptables.Cards.Abstractions;
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
            Debug.Log($"Healed self for {_healAmount}!");
        }
    }
}