using System;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using StateMachine;

namespace Player.Hand.States
{
    [Serializable]
    public class HandEnemySelectionState: IState
    {
        [OdinSerialize]
        private ICardSelectionHandler _cardSelectionHandler;
        public void Enter()
        {
            _cardSelectionHandler.BlockSelection(true);
        }

        public void Exit()
        {
            _cardSelectionHandler.BlockSelection(false);
        }

        public void Tick()
        {
            
        }
    }
}