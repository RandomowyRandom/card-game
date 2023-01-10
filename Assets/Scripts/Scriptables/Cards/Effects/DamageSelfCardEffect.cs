using System;
using Scriptables.Cards.Abstractions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class DamageSelfCardEffect : ICardEffect
    {
        [SerializeField]
        private int _damage;
        public void OnUse()
        {
            Debug.Log($"Damaging self for {_damage}!");
        }
    }
}