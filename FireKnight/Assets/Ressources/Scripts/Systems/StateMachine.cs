using System.Collections;
using System.Collections.Generic;
using System;

namespace Dim.Structures
{

    /////////////////////////////////////////////////
    /// Base Class to create StateMachines.
    /////////////////////////////////////////////////
    public class StateMachine<StateType,State> where StateType : struct where State : State<StateType> 
    {
        public delegate void TransitionDelegate(StateType from, StateType to);
        public event TransitionDelegate OnTransitionEvent;

        protected StateType currentState;
        protected Dictionary<StateType, State> fsm;
        protected bool firstFrameOfStateChange;

        public StateMachine()
        {
            fsm = new Dictionary<StateType, State>();
            firstFrameOfStateChange = true;
        }

        public StateType CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public Dictionary<StateType, State> GetDict()
        {
            return fsm;
        }

        public void Update()
        {
            CheckAndCorrectState();
            fsm[CurrentState].Update();
        }

        private void CheckAndCorrectState()
        {

            firstFrameOfStateChange = false;

            foreach (var transition in fsm[CurrentState].transitions)
            {
                if (transition.TransitionShouldOccur())
                {
                    
                    OnTransitionEvent?.Invoke(currentState, transition.resultingState);

                    currentState = transition.resultingState;
                    firstFrameOfStateChange = true;


                    break;
                }
            }
        }

    }

    /////////////////////////////////////////////////
    /// Used by StateMachine, base class for StateMachines State.
    /////////////////////////////////////////////////
    public class State<StateType> where StateType : struct
    {

        public readonly StateType type;
        public readonly Transition<StateType>[] transitions;
        public readonly Action Update;

        public State(StateType _type, Action _Update, Transition<StateType>[] _Transitions)
        {
            type = _type;
            transitions = _Transitions;
            Update = _Update;
        }
    }

    /////////////////////////////////////////////////
    /// Used by StateMachine, base class for StateMachines Transitions.
    /////////////////////////////////////////////////
    public class Transition<StateType> where StateType :struct
    {

        public readonly StateType resultingState;
        public readonly Func<bool> TransitionShouldOccur;

        public Transition(StateType _resultingState, Func<bool> _TransitionShouldOccur)
        {
            resultingState = _resultingState;
            TransitionShouldOccur = _TransitionShouldOccur;
        }
    }
}
