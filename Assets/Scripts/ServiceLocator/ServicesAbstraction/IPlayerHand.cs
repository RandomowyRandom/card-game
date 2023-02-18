using System;
using System.Collections.Generic;
using Scriptables.Cards.Abstractions;
using Scriptables.Player;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerHand: IService
    {
        public event Action<Card> OnCardAdded;
        
        public event Action<Card, int> OnCardRemoved;
        
        public event Action OnHandCleared;
        
        public event Action OnCardKeysAdded;
        
        public event Action OnCardKeysRemoved;
        
        public event Action OnCardKeysCleared;
        
        public List<Card> Cards { get; }
        public int CardKeysCount { get; }

        public void UpgradeCard(Card card);
        public bool UpgradeDeck();
        public bool UpgradeRandomCard();
        public void AddCard(Card card);
        public void RemoveCard(Card card);
        public void SetCardDrawConfiguration(PlayerCardDrawConfiguration configuration);
    }
}