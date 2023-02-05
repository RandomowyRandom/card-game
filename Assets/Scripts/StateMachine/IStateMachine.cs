using System;

namespace StateMachine
{
    public interface IStateMachine
    {
        public event Action<IState, IState> OnStateChanged;
        
        public IState CurrentState { get; }
        public void SetState(IState state);
    }
}