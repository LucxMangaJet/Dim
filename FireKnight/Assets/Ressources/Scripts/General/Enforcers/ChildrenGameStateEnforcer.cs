using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Enforcer
{
    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED]
    /////////////////////////////////////////////////
    public class ChildrenGameStateEnforcer : MonoBehaviour
    {
        

        [SerializeField]
        private GameState.State requiredState;

        void Start()
        {
            GameState.instance.StateChange += OnGameStateChange;
        }

        void OnGameStateChange(GameState.State state)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(state == requiredState);
            }

        }

        void OnDestroy()
        {
            GameState.instance.StateChange -= OnGameStateChange;

        }
    }
}