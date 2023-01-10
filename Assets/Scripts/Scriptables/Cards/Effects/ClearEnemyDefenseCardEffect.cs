using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class ClearEnemyDefenseCardEffect : ICardEffect
    {
        public void OnUse()
        {
            Debug.Log("Cleared enemy Defense");
        }
    }
}