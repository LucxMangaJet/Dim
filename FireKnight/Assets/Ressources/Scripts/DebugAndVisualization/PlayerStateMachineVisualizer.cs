using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Visualizes the PlayerStateMachine
    /////////////////////////////////////////////////
    public class PlayerStateMachineVisualizer : MonoBehaviour
    {


        [SerializeField] bool visualize;
        [SerializeField] Vector3[] statesPos;

        private StateMachineVisualizer<Player.StateType, Player.PlayerState> visualizer;


        //Called by PlayerController SendMessage on Start
        private void GetFSM(Player.PlayerStateMachine fSM)
        {
            visualizer = new StateMachineVisualizer<Player.StateType, Player.PlayerState>(fSM,transform, statesPos);
        }

        private void OnDrawGizmos()
        {
            if (visualize && visualizer != null)
            {
                visualizer.Display();
            }

        }


    }
}


