using System;
using StateMachine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandEnemySelectionState: IState
    {
        private ICardSelectionHandler _cardSelectionHandler;
        
        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        public void Enter()
        {
            // TODO: block end turn button
            CardSelectionHandler.BlockSelection(true);
        }

        public void Exit()
        {
            // TODO: unblock end turn button
            CardSelectionHandler.BlockSelection(false);
        }

        public void Tick()
        {
            
        }
    }
}