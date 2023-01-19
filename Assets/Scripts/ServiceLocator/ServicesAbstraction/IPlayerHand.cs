using System;
using System.Collections.Generic;
using Scriptables.Cards.Abstractions;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerHand: IService
    {
        public event Action<Card> OnCardAdded;
        
        public event Action<Card, int> OnCardRemoved;
        
        public event Action OnHandCleared;
        
        public event Action<int> OnCardKeysAdded;
        
        public event Action<int> OnCardKeysRemoved;
        
        public event Action<int> OnCardKeysCleared;
        
        public List<Card> Cards { get; }

        public void UpgradeCard(Card card);
        public bool UpgradeDeck();
        public bool UpgradeRandomCard();
        public void AddCard(Card card);
        public void RemoveCard(Card card);
    }
}