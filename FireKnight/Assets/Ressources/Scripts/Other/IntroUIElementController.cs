using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{
    public class IntroUIElementController : MonoBehaviour
    {
        [SerializeField] float maxDist=10;
        [SerializeField] AnimationCurve animationCurve;

        SpriteRenderer sRenderer;

        void Start()
        {
            sRenderer = GetComponent<SpriteRenderer>();
        }

        
        void Update()
        {
            DistanceBasedAlpha();
        }

        private void DistanceBasedAlpha()
        {
            Color c = sRenderer.color;

            float dist = Vector3.Distance(transform.position, LevelHandler.GetPlayer().position);
            float val = 1;

            val = 1- Mathf.Min(1, dist / maxDist);
            
            c.a = animationCurve.Evaluate(val);
            sRenderer.color = c;
        }
    }
}
