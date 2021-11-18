using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoaT.AI
{
    public class StateManager
    {
        private Dictionary<Type, State> _availableStates;

        public State CurrentState { get { return _currentState; } }

        private State _currentState;

        public void SetStates(Dictionary<Type,State> states, State initialState)
        {
            _availableStates = states;

            if (states.ContainsKey(initialState.GetType()))
            {
                _currentState = states[initialState.GetType()];            
                CurrentState.Awake();
            }
            else
                Debug.LogError("The initial state was not in the states dictionary;");
        }

        public void Update()
        {
            if (CurrentState != null)
                _currentState.Execute();
        }

        public void SetState<T>() where T : State
        {
            //Debug.Log($"I am setting state {typeof(T)}");s
            if (_availableStates.ContainsKey(typeof(T)) && CurrentState.GetType() != typeof(T))
            {
                CurrentState.Sleep();
                _currentState = _availableStates[typeof(T)];
                CurrentState.Awake();

                EventManager.Raise(EventsData.OnCharacterStateChange, typeof(T));
            }
            else if (!_availableStates.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"No State of type {typeof(T)} found in this StateManager.");
            }
        }

        public void AddState(State state)
        {
            _availableStates.Add(state.GetType(), state);
        }

        public bool IsActualState<T>() where T : State
        {
            return CurrentState.GetType() == typeof(T);
        }
    }
}