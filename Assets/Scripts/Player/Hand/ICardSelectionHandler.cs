using System;
using Cards;
using ServiceLocator;

namespace Player.Hand
{
    public interface ICardSelectionHandler: IService
    {
        public event Action<CardWorld> OnSelected; 
        public event Action<CardWorld> OnDeselected;
        
        public CardWorld SelectedCard { get; }
        
        public void BlockSelection(bool block);
    }
}