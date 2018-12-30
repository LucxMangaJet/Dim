using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Interaction;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Visualization for the Platform, turns a light on/off based on usability.
    /////////////////////////////////////////////////
    public class PlatformLook : MonoBehaviour
    {

        public bool isVisible;
        Animator animator;
        MovingEnvironmentController wheel;
        Light light;
        

        // Use this for initialization
        void Start()
        {
            isVisible = false;

            animator = GetComponentInChildren<Animator>();
            wheel = GetComponentInChildren<MovingEnvironmentController>();
            light = GetComponentInChildren<Light>();

            GetComponent<Platform>().OnIsActiveChange += UpdateVisibility;

            UpdateVisibility(false);
        }

        public void UpdateVisibility(bool isActive)
        {
            animator.enabled = isActive;
            wheel.enabled = isActive;
            light.enabled = isActive;
            isVisible = isActive;
        }
    }
}