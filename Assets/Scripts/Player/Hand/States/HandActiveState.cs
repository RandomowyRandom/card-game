using System;
using Common.Attributes;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using StateMachine;
using UnityEngine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandActiveState: IState
    {
        private ICardSelectionHandler _cardSelectionHandler;
        private IRoundButton _roundButton;

        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        private IRoundButton RoundButton => _roundButton ??= ServiceLocator.ServiceLocator.Instance.Get<IRoundButton>();
        public void Enter()
        {
            RoundButton.BlockButton(false);
            CardSelectionHandler.BlockSelection(false);
        }

        public void Exit()
        {
            RoundButton.BlockButton(true);
            CardSelectionHandler.BlockSelection(true);
        }

        public void Tick()
        {
            
        }
    }
}