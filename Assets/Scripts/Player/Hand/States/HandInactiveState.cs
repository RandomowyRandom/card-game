using System;
using Sirenix.Serialization;
using StateMachine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandInactiveState: IState
    {
        private ICardSelectionHandler _cardSelectionHandler;
        
        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        public void Enter()
        {
            CardSelectionHandler.BlockSelection(true);
        }

        public void Exit()
        {
            CardSelectionHandler.BlockSelection(false);
        }

        public void Tick()
        {
            
        }
    }
}