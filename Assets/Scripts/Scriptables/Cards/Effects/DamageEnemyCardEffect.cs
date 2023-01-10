using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DamageEnemyCardEffect : ICardEffect
    {
        [SerializeField]
        private int _damage;
        
        public void OnUse()
        {
            Debug.Log($"Deal {_damage} damage!");
        }
    }
}