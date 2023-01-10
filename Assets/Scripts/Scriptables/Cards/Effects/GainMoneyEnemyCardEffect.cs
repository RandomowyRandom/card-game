using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class GainMoneyEnemyCardEffect : ICardEffect
    {
        [SerializeField]
        private int _money;
        public void OnUse()
        {
            Debug.Log($"Enemy gained {_money} money");
        }
    }
}