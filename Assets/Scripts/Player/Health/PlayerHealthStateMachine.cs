using System;
using StateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerHealthStateMachine: MonoBehaviour, IStateMachine
    {
        public event Action<IState, IState> OnStateChanged;
        public IState CurrentState => _currentState;
        
        private IState _currentState;
        public void SetState(IState state)
        {
            var oldState = _currentState;
            
            _currentState?.Exit();
            
            _currentState = state;
            
            _currentState.Enter();
            OnStateChanged?.Invoke(oldState, state);
        }
        
        private void Update()
        {
            _currentState?.Tick();
        }
    }
}