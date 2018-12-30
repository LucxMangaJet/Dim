using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{


    /////////////////////////////////////////////////
    /// Visualizes the PickGoatStateMachine
    /////////////////////////////////////////////////
    public class PickGoatStateMachineVisualizer : MonoBehaviour
    {

        [SerializeField] bool visualize;
        [SerializeField] Vector3[] statesPos;

        private StateMachineVisualizer<Enemies.PickGoatStateType, Enemies.PickGoatState> visualizer;


        //Called by PickGoatController SendMessage on Start
        private void GetFSM(Enemies.PickGoatStateMachine fSM)
        {
            visualizer = new StateMachineVisualizer<Enemies.PickGoatStateType, Enemies.PickGoatState>(fSM, transform, statesPos);
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