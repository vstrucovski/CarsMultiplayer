using System;
using System.Collections.Generic;

namespace Scripts.Shared
{
    public class SimpleFSM<TState> where TState : Enum
    {
        private TState _currentState;
        private readonly Dictionary<TState, Action> _onEnter = new();
        private readonly Dictionary<TState, Action> _onExit = new();

        public TState CurrentState => _currentState;

        public void AddState(TState state, Action onEnter = null, Action onExit = null)
        {
            if (onEnter != null)
                _onEnter[state] = onEnter;
            
            if (onExit != null)
                _onExit[state] = onExit;
        }

        public void SwitchState(TState newState)
        {
            if (EqualityComparer<TState>.Default.Equals(_currentState, newState))
                return;

            if (_onExit.TryGetValue(_currentState, out var exit))
                exit?.Invoke();

            _currentState = newState;

            if (_onEnter.TryGetValue(_currentState, out var enter))
                enter?.Invoke();
        }
    }
}