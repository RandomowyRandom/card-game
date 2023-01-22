using System;
using Cards;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;

namespace Scriptables.Cards.Effects
{
    [Serializable]
    public class CardDrawRarityCardEffect: ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }
        
        [field: OdinSerialize]
        private int Amount { get; }
        
        [field: OdinSerialize]
        private CardRarity[] _cardRarities;
        public void OnUse()
        {
            var targets = TargetProvider.GetTargets();

            foreach (var target in targets)
            {
                var hand = target.GetComponent<IPlayerHand>();
                var deck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();
                
                for (var i = 0; i < Amount; i++)
                {
                    var card = deck.DrawCard(_cardRarities);
                    hand.AddCard(card);
                }
            }
        }
    }
}