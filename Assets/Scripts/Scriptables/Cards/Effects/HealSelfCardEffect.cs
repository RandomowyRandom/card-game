using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class HealSelfCardEffect: ICardEffect
    {
        [SerializeField]
        private int _healAmount;
        
        public void OnUse()
        {
            Debug.Log($"Healed self for {_healAmount}!");
        }
    }
}