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
        [OdinSerialize]
        private ICardSelectionHandler _cardSelectionHandler;
        public void Enter()
        {
            _cardSelectionHandler.BlockSelection(false);
        }

        public void Exit()
        {
            _cardSelectionHandler.BlockSelection(true);
        }

        public void Tick()
        {
            
        }
    }
}