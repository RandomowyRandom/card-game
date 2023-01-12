using System;
using Cards;
using Mirror;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Deck
{
    public class CardDeck : NetworkBehaviour, ICardDeck
    {
        [SerializeField]
        private CardDatabase _cardDatabase;
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<ICardDeck>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<ICardDeck>();
        }

        public Card DrawCard()
        {
            var card = _cardDatabase.GetRandomCard();
            return card;
        }

        public Card DrawCard(CardRarity rarity)
        {
            return _cardDatabase.GetRandomCard(rarity);
        }

        public Card DrawCard(CardRarity[] rarities)
        {
            return _cardDatabase.GetRandomCard(rarities);
        }

        public void ReturnCard(Card card)
        {
            throw new NotImplementedException();
        }
    }
}