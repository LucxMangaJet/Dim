using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dim;


namespace Dim.Enforcer
{

    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED]
    /////////////////////////////////////////////////
    public class AdvancedGameStateEnforcer : MonoBehaviour
    {

        [SerializeField]
        AdvancedGameStatePair[] componentsToControl;
        [SerializeField]
        AdvancedGameStateFunctionsPair[] functionsToCall;

        void Start()
        {
            if (componentsToControl.Length > 0)
            {
                GameState.instance.StateChange += ControlComponents;
            }
            if (functionsToCall.Length > 0)
            {
                GameState.instance.StateChange += ControlFunctions;
            }
        }

	void OnDestroy()
	{
	   if (componentsToControl.Length > 0)
            {
                GameState.instance.StateChange -= ControlComponents;
            }
            if (functionsToCall.Length > 0)
            {
                GameState.instance.StateChange -= ControlFunctions;
            }
	}

        void ControlComponents(GameState.State state)
        {
            foreach (var pair in componentsToControl)
            {
                if (pair.behaviours.Length > 0)
                {
                    foreach (var behavior in pair.behaviours)
                    {
                        behavior.enabled = state == pair.state;
                    }
                }
            }
        }

        void ControlFunctions(GameState.State state)
        {
            foreach (var pair in functionsToCall)
            {
                if (state == pair.state)
                {
                    pair.funcions?.Invoke();
                }
            }
        }


    }

    [System.Serializable]
    internal struct AdvancedGameStatePair
    {
        public GameState.State state;
        public Behaviour[] behaviours;
    }

    [System.Serializable]
    internal struct AdvancedGameStateFunctionsPair
    {
        public GameState.State state;
        public UnityEvent funcions;
    }
}