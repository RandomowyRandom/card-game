using System;
using Common.Attributes;
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
        
        private ICardSelectionHandler CardSelectionHandler => _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        public void Enter()
        {
            CardSelectionHandler.BlockSelection(false);
        }

        public void Exit()
        {
            CardSelectionHandler.BlockSelection(true);
        }

        public void Tick()
        {
            
        }
    }
}