using System;
using Scriptables.Cards.Abstractions;
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
            Debug.Log($"Deal {_damage} damage!");
        }
    }
}