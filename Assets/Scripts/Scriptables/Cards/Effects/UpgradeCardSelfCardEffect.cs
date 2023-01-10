using System;
using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class UpgradeCardSelfCardEffect : ICardEffect
    {
        public void OnUse()
        {
            Debug.Log("Upgraded card!");
        }
    }
}