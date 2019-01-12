using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Rotates an object at the given speed.
    /////////////////////////////////////////////////
    public class MovingEnvironmentController : MonoBehaviour
    {

        [SerializeField] Vector3 customRotation;
        public float speed = 1;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(customRotation * speed*Time.deltaTime*60);
        }
    }

}