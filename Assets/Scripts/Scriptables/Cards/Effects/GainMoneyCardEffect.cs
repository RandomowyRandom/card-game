using System;
using Scriptables.Cards.Abstractions;
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
            Debug.Log($"Enemy gained {_money} money");
        }
    }
}