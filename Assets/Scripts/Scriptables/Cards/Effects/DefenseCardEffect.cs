using System;
using Scriptables.Cards.Abstractions;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DefenseCardEffect : ICardEffect
    {
        [SerializeField]
        private int _defense;

        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            Debug.Log($"Added {_defense} to player's defense!");
        }
    }
}