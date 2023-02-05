﻿using Sirenix.OdinInspector;

namespace StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Tick();
    }
}