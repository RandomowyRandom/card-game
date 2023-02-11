using System;
using ServiceLocator.ServicesAbstraction;
using StateMachine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandEnemySelectionState: IState
    {
        private ICardSelectionHandler _cardSelectionHandler;
        private IRoundButton _roundButton;
        
        private IRoundButton RoundButton => _roundButton ??= ServiceLocator.ServiceLocator.Instance.Get<IRoundButton>();
        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        
        public void Enter()
        {
            RoundButton.BlockButton(true);
            CardSelectionHandler.BlockSelection(true);
        }

        public void Exit()
        {
            RoundButton.BlockButton(true);
            CardSelectionHandler.BlockSelection(false);
        }

        public void Tick()
        {
            
        }
    }
}