using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Base Class for Interactions, establishes a connection to a Storage Object.
    /////////////////////////////////////////////////
    public class InteractionBase : MonoBehaviour, Visualize.IExtraVisualization
    {

        public bool debug;
        public Storage Storage;
        protected Queue<System.Action> operationsQueue = new Queue<System.Action>();

        public virtual void Awake()
        {
            if (Storage!=null)
            {
                Storage.OnEnergyChange += OnEnergyChange;
                operationsQueue = new Queue<System.Action>();
            }
            else
            {
                Debug.Log("No Storage connected to " + gameObject.name);
            }
        }

        public virtual void OnEnergyChange(byte newEnergy)
        {
            Debug.Log("Unimplemented EnergyChange on" + gameObject.name);
        }

        protected virtual void OnDrawGizmos()
        {
            if (debug && Storage != null)
            {
                Gizmos.DrawLine(transform.position, Storage.transform.position);
            }
        }

        public virtual string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                (Storage!=null)? "Linked to " + Storage.name: "UNLINKED"
            };
        }
    }
}
