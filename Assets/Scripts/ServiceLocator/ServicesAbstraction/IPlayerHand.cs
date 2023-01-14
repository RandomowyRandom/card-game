using System;
using System.Collections.Generic;
using Scriptables.Cards.Abstractions;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerHand: IService
    {
        public event Action OnHandChanged;
        
        public List<Card> Cards { get; }
        
        public void AddCard(Card card);
        public void RemoveCard(Card card);
    }
}