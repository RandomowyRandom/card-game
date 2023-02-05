using System;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using StateMachine;

namespace Player.Hand.States
{
    public class PlayerHandStateMachine: NetworkBehaviour, IPlayerHandStateMachine
    {
        public event Action<IState, IState> OnStateChanged;
        public IState CurrentState => _currentState;
        private IState _currentState;

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerHandStateMachine>(this);
        }

        private void Start()
        {
            SetState(new HandInactiveState());
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerHandStateMachine>();
        }

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