using System;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class CardUpgradeCardEffect : ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }

        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();
            
            foreach (var target in targets)
            {
                var playerHand = target.GetComponent<IPlayerHand>();
                var result = playerHand.UpgradeRandomCard();

                Debug.Log(result ? "Card upgraded" : "No cards to upgrade");
            }
        }
    }
}