using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{


    /////////////////////////////////////////////////
    /// Used to Visualize the CoreDrainer StateMachine.
    /////////////////////////////////////////////////
    public class CoreDrainerStateMachineVisualizer : MonoBehaviour
    {

        [SerializeField] bool visualize;
        [SerializeField] Vector3[] statesPos;

        private StateMachineVisualizer<Enemies.CoreDrainerStateType, Enemies.CoreDrainerState> visualizer;


        //Called by PickGoatController SendMessage on Start
        private void GetFSM(Enemies.CoreDrainerStateMachine fSM)
        {
            visualizer = new StateMachineVisualizer<Enemies.CoreDrainerStateType, Enemies.CoreDrainerState>(fSM,transform, statesPos);
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
