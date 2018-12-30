using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim;


namespace Dim.Enforcer
{
    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED]
    /////////////////////////////////////////////////
    public class SimpleGameStateEnforcer : MonoBehaviour
    {

        [SerializeField]
        private GameState.State requiredState;

        void Start()
        {
            GameState.instance.StateChange += OnGameStateChange;
        }

        void OnGameStateChange(GameState.State state)
        {

            foreach (Behaviour component in gameObject.GetComponents(typeof(Behaviour)))
            {
                component.enabled = state == requiredState;
            }

        }

	void OnDestroy()
	{
	    GameState.instance.StateChange -= OnGameStateChange;

	}
    }
}