using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Structures;
using System;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Base Class to Visualize StateMachines through Gizmos
    /////////////////////////////////////////////////
    public class StateMachineVisualizer<StateType, State> where StateType : struct where State : Dim.Structures.State<StateType>
    {
        public static int FONT_SIZE = 9 ;
        public static float LINE_WIDTH = 2;
        public static float TRANSITION_DISPLAY_TIME = 1;
        public static float LINE_DISPLACEMENT_AMOUNT = 4;

        StateMachine<StateType, State> FSM;
        Dictionary<StateType, FSMVisualizerObject<StateType>> objs;
        public readonly Vector3[] statesPos;
        private Transform origin;

        public StateMachineVisualizer(StateMachine<StateType, State> _FSM,Transform t, Vector3 [] _statesPos)
        {
            FSM = _FSM;
            statesPos = _statesPos;
            origin = t;
            var FSMDict = FSM.GetDict();

            if (FSMDict.Count != statesPos.Length)
            {
                throw new Exception("FSMVisualizer Error: The given States Positions array does not have the same length as the FSM. Current size: " + statesPos.Length + ". Expected size: " + FSMDict.Count);
            }

            objs = new Dictionary<StateType, FSMVisualizerObject<StateType>>();
            int counter = 0;

            foreach (var state in FSMDict)
            {
                objs.Add(state.Key, new FSMVisualizerObject<StateType>(statesPos[counter], FSMVisualizerTransition<StateType>.CreateArrayFromTransitions(state.Value.transitions)));

                counter++;
            }
            FSM.OnTransitionEvent += UpdateTransitionTimers;
        }

        private void UpdateTransitionTimers(StateType start, StateType end)
        {
            foreach (var transition in objs[start].transitions)
            {
                if (transition.endName.ToString() == end.ToString())
                {
                    transition.timeSinceLastUsed = 0;
                }
            }

        }

        public void Display()
        {
            if (FSM == null)
            {
                return;
            }

            foreach (var state in objs)
            {

                foreach (var transition in state.Value.transitions)
                {
                    transition.timeSinceLastUsed += Time.deltaTime; 
             
                    Color d = (transition.timeSinceLastUsed < TRANSITION_DISPLAY_TIME) ? Color.green : new Color(1,1,1,0.2f);

                    GizmosUtils.DrawArrow(GUI.skin, origin.position + state.Value.displacement, origin.position + objs[transition.endName].displacement, LINE_WIDTH, d);

                }

                Color c = (state.Key.ToString() == FSM.CurrentState.ToString()) ? Color.green : new Color(1, 1, 1, 0.6f);
                GUI.color = c;

                GizmosUtils.DrawText(GUI.skin, state.Key.ToString(),state.Value.displacement + origin.position, c, FONT_SIZE);
            }
        }


    }


    /////////////////////////////////////////////////
    /// Used by the StateMachineVisualizer, represents a FSMs Node
    /////////////////////////////////////////////////
    public class FSMVisualizerObject<StateType> where StateType : struct
    {
        public readonly Vector3 displacement;
        public FSMVisualizerTransition<StateType>[] transitions;
        public FSMVisualizerObject(Vector3 disp, FSMVisualizerTransition<StateType>[] _transitions)
        {
            displacement = disp;
            transitions = _transitions;
        }
    }

    /////////////////////////////////////////////////
    /// Used by the StateMachineVisualizer, represents a FSMs Connection
    /////////////////////////////////////////////////
    public class FSMVisualizerTransition<StateType> where StateType : struct
    {
        public readonly StateType endName;
        public readonly string funcName;
        public float timeSinceLastUsed;

        public FSMVisualizerTransition(StateType _endName, string _funcName)
        {
            endName = _endName;
            funcName = _funcName;
            timeSinceLastUsed = float.MaxValue;
        }

        public static FSMVisualizerTransition<StateType>[] CreateArrayFromTransitions( Transition<StateType>[] tr)
        {
            FSMVisualizerTransition<StateType>[] arr = new FSMVisualizerTransition<StateType>[tr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
               arr[i] = new FSMVisualizerTransition<StateType>(tr[i].resultingState, tr[i].TransitionShouldOccur.ToString());
            }

            return arr;
        }
    }

}
