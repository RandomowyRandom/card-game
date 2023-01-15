using System;
using Cards;

namespace Player.Hand
{
    public interface ICardSelectionHandler
    {
        public event Action<CardWorld> OnSelected; 
        public event Action<CardWorld> OnDeselected;
        
        public CardWorld SelectedCard { get; }
    }
}