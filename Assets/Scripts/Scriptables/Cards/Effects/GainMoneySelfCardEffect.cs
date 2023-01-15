using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class GainMoneySelfCardEffect : ICardEffect
    {
        [SerializeField]
        private int _money;

        public void OnUse()
        {
            Debug.Log($"Gained {_money} money!");
        }
    }
}