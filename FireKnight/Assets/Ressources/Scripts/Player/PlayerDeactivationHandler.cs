using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Used to deactivate the Player model (like on Death) while still keeping necessary components active.
    /////////////////////////////////////////////////
    public class PlayerDeactivationHandler : MonoBehaviour
    {

        [SerializeField] GameObject[] toKeepActive;
        [SerializeField] Behaviour[] behavioursToKeepActive;


        public void Dectivate()
        {
            //for some reason colliders are not behaviours which results in a duplication of the "enabled" property. There isnt even an interface for it..
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            List<Behaviour> behaviours = new List<Behaviour>(behavioursToKeepActive);
            foreach (var c in gameObject.GetComponents<Behaviour>())
            {
                if (!behaviours.Contains(c))
                {
                    c.enabled = false;
                }
            }

            List<GameObject> gameObjects = new List<GameObject>(toKeepActive);
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject g = transform.GetChild(i).gameObject;

                if (!gameObjects.Contains(g))
                {
                    g.SetActive(false);
                }
            }

        }

        public void Activate()
        {

            foreach (var c in gameObject.GetComponents<Behaviour>())
            {
                c.enabled = true;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject g = transform.GetChild(i).gameObject;

                g.SetActive(true);
            }

            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
