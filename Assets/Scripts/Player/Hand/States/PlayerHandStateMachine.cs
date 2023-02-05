using System;
using Sirenix.OdinInspector;
using StateMachine;

namespace Player.Hand.States
{
    public class PlayerHandStateMachine: SerializedMonoBehaviour, IStateMachine
    {
        public event Action<IState, IState> OnStateChanged;
        public IState CurrentState => _currentState;

        private IState _currentState;
        
        public void SetState(IState state)
        {
            _currentState?.Exit();
            
            _currentState = state;
            
            _currentState.Enter();
            OnStateChanged?.Invoke(CurrentState, state);
        }
        
        private void Update()
        {
            _currentState?.Tick();
        }
    }
}