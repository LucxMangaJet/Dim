using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Used by the Npcs AI to make sure they are not going to walk off an edge.
    /////////////////////////////////////////////////
    public class GroundChecker : MonoBehaviour, Visualize.IExtraVisualization
    {

        Queue<bool> collection; 
        int counter = 0;

        private void Start()
        {
            collection = new Queue<bool>();
        }

        private void Update()
        {
            bool onGround = false;
            if (counter > 0)
            {
                onGround = true;
                counter = 0;
            }

            collection.Enqueue(onGround);

            if (collection.Count > 5)
            {
                collection.Dequeue();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.isTrigger)
                counter++;
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[] {IsOnGround().ToString()};
        }

        public bool IsOnGround()
        {
            foreach (var item in collection)
            {
                if (item)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
