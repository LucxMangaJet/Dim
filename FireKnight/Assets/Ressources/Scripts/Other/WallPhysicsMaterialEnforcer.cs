using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim
{

    /////////////////////////////////////////////////
    /// Enforcers the Platforms to be slippery when Positioned vertically.
    /////////////////////////////////////////////////
    public class WallPhysicsMaterialEnforcer : MonoBehaviour
    {

        [SerializeField] PhysicMaterial physicMaterial;


        void Start()
        {
            if (transform.eulerAngles.z > 80)
            {
                Collider collider = GetComponent<Collider>();
                collider.material = physicMaterial;
            }
            Destroy(this);
        }


    }
}
