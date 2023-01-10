using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DamageAllCardEffect: ICardEffect
    {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private bool _includeCaster;
        public void OnUse()
        {
            var includeCaster = _includeCaster ? "including caster" : "excluding caster";
            Debug.Log($"Damage all for {_damage} {includeCaster}");            
        }
    }
}