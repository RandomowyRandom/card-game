using System;
using Cysharp.Threading.Tasks;
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
        public bool IsAsync => TargetProvider is IAsyncTargetProvider;


        public async UniTask OnUse()
        {
            var targets = TargetProvider switch
            {
                ISyncTargetProvider syncTargetProvider => syncTargetProvider.GetTargets(),
                IAsyncTargetProvider asyncTargetProvider => await asyncTargetProvider.GetTargets(),
                _ => null
            };
            
            foreach (var target in targets!)
            {
                var playerHand = target.GetComponent<IPlayerHand>();
                var result = playerHand.UpgradeRandomCard();

                Debug.Log(result ? "Card upgraded" : "No cards to upgrade");
            }
        }
    }
}