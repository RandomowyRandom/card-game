using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DefenseSelfCardEffect : ICardEffect
    {
        [SerializeField]
        private int _defense;
        public void OnUse()
        {
            Debug.Log($"Added {_defense} to player's defense!");
        }
    }
}