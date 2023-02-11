using System;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;
using StateMachine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandInactiveState: IState
    {
        private ICardSelectionHandler _cardSelectionHandler;
        private IRoundButton _roundButton;

        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        private IRoundButton RoundButton => _roundButton ??= ServiceLocator.ServiceLocator.Instance.Get<IRoundButton>();
        public void Enter()
        {
            RoundButton.BlockButton(true);
            CardSelectionHandler.BlockSelection(true);
        }

        public void Exit()
        {
            RoundButton.BlockButton(false);
            CardSelectionHandler.BlockSelection(false);
        }

        public void Tick()
        {
            
        }
    }
}